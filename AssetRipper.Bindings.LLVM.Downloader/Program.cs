using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AssetRipper.Bindings.LLVM.Downloader;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		if (args.Length != 2)
		{
			Console.WriteLine("This program takes exactly two arguments:");
			Console.WriteLine("1. The LLVM version.");
			Console.WriteLine("2. The target.");
			return;
		}

		string version = args[0];

		Target target = ParseTarget(args[1]);
		if (target is Target.Unknown)
		{
			Console.WriteLine($"Unknown target: {args[1]}");
			return;
		}
		if (target is Target.WindowsX86 or Target.MacOsX64)
		{
			Console.WriteLine($"Unsupported target: {args[1]}");
			return;
		}

		string? url = GetUrl(version, target);
		if (url is null)
		{
			Console.WriteLine($"Could not get URL for version {version} and target {args[1]}");
			return;
		}

		string tempFilePath = Path.Join(AppContext.BaseDirectory, "llvm.tar.xz");

		// Download the file
		{
			Console.WriteLine($"Downloading LLVM {version} for {args[1]}...");
			using HttpClient client = new();
			client.DefaultRequestHeaders.UserAgent.Add(new(typeof(Program).Namespace!, typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0"));
			using Stream stream = await client.GetStreamAsync(url);
			using FileStream fileStream = File.Create(tempFilePath);
			await stream.CopyToAsync(fileStream);
		}

		// Extract the file
		{
			string outputDirectory = Path.Join(AppContext.BaseDirectory, "llvm");
			Console.WriteLine($"Extracting to {Path.GetFullPath(outputDirectory)}...");
			using IReader reader = OpenReader(tempFilePath, target);
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			Directory.CreateDirectory(outputDirectory);
			ExtractionOptions options = new()
			{
				ExtractFullPath = true,
				Overwrite = true,
			};
			List<string> filesToDelete = [];
			while (reader.MoveToNextEntry())
			{
				IEntry entry = reader.Entry;
				string? key = entry.Key;
				Console.WriteLine(key);
				if (key is null)
				{
					continue;
				}

				if (key.StartsWith("bin/"))
				{
					if (!IsWindows(target))
					{
						continue;
					}
					bool isInSubDir = key.IndexOf('/', 4) >= 0;
					if (isInSubDir)
					{
						continue;
					}
					string? fileExtension = Path.GetExtension(key);
					if (fileExtension is not ".dll")
					{
						continue;
					}

					// We need to delete files like LLVM-C.lib because they have duplicate symbols.
					filesToDelete.Add(Path.Join(outputDirectory, "lib", Path.GetFileNameWithoutExtension(key) + ".lib"));
				}
				else if (key.StartsWith("lib/"))
				{
					bool isInSubDir = key.IndexOf('/', 4) >= 0;
					if (!isInSubDir)
					{
						reader.WriteEntryToDirectory(outputDirectory, options);
					}
				}
				else if (key.StartsWith("include/"))
				{
					reader.WriteEntryToDirectory(outputDirectory, options);
				}
			}
			foreach (string file in filesToDelete)
			{
				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
		}

		// Clean up
		{
			Console.WriteLine("Cleaning up...");
			File.Delete(tempFilePath);
		}
	}

	private static bool IsWindows(Target target) =>
		target is Target.WindowsX64 or Target.WindowsX86 or Target.WindowsArm64;

	// https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
	private static Target ParseTarget(string targetString) => targetString switch
	{
		"win-x64" => Target.WindowsX64,
		"win-x86" => Target.WindowsX86,
		"win-arm64" => Target.WindowsArm64,
		"linux-x64" => Target.LinuxX64,
		"linux-arm64" => Target.LinuxArm64,
		"osx-x64" => Target.MacOsX64,
		"osx-arm64" => Target.MacOsArm64,
		_ => Target.Unknown,
	};

	private static string? GetUrl(string version, Target target) => target switch
	{
		Target.WindowsX64 => $"https://github.com/llvm/llvm-project/releases/download/llvmorg-{version}/clang+llvm-{version}-x86_64-pc-windows-msvc.tar.xz",
		Target.WindowsArm64 => $"https://github.com/llvm/llvm-project/releases/download/llvmorg-{version}/clang+llvm-{version}-aarch64-pc-windows-msvc.tar.xz",
		Target.LinuxX64 => $"https://github.com/llvm/llvm-project/releases/download/llvmorg-{version}/LLVM-{version}-Linux-X64.tar.xz",
		Target.LinuxArm64 => $"https://github.com/llvm/llvm-project/releases/download/llvmorg-{version}/LLVM-{version}-Linux-ARM64.tar.xz",
		Target.MacOsArm64 => $"https://github.com/llvm/llvm-project/releases/download/llvmorg-{version}/LLVM-{version}-macOS-ARM64.tar.xz",
		_ => null,
	};

	private static IReader OpenReader(string path, Target target)
	{
		IReader reader = ReaderFactory.Open(path);
		if (target is Target.WindowsX64 or Target.WindowsArm64)
		{
			return new LlvmReader(reader, "clang+llvm-");
		}
		else
		{
			return new LlvmReader(reader, "LLVM-");
		}
	}

	private sealed class LlvmEntry(IEntry Original, string Prefix) : IEntry
	{
		CompressionType IEntry.CompressionType => Original.CompressionType;

		DateTime? IEntry.ArchivedTime => Original.ArchivedTime;

		long IEntry.CompressedSize => Original.CompressedSize;

		long IEntry.Crc => Original.Crc;

		DateTime? IEntry.CreatedTime => Original.CreatedTime;

		string? IEntry.Key
		{
			get
			{
				string? key = Original.Key;
				Debug.Assert(key is not null);
				Debug.Assert(key.StartsWith(Prefix, StringComparison.Ordinal));
				int firstSlash = key.IndexOf('/');
				if (firstSlash < 0 || firstSlash + 1 >= key.Length)
				{
					return null;
				}
				return key[(firstSlash + 1)..];
			}
		}

		string? IEntry.LinkTarget => Original.LinkTarget;

		bool IEntry.IsDirectory => Original.IsDirectory;

		bool IEntry.IsEncrypted => Original.IsEncrypted;

		bool IEntry.IsSplitAfter => Original.IsSplitAfter;

		bool IEntry.IsSolid => Original.IsSolid;

		int IEntry.VolumeIndexFirst => Original.VolumeIndexFirst;

		int IEntry.VolumeIndexLast => Original.VolumeIndexLast;

		DateTime? IEntry.LastAccessedTime => Original.LastAccessedTime;

		DateTime? IEntry.LastModifiedTime => Original.LastModifiedTime;

		long IEntry.Size => Original.Size;

		int? IEntry.Attrib => Original.Attrib;
	}

	private sealed class LlvmReader(IReader Original, string Prefix) : IReader
	{
		ArchiveType IReader.ArchiveType => Original.ArchiveType;

		IEntry IReader.Entry => new LlvmEntry(Original.Entry, Prefix);
		bool IReader.Cancelled => Original.Cancelled;

		void IReader.Cancel()
		{
			Original.Cancel();
		}

		void IDisposable.Dispose()
		{
			Original.Dispose();
		}

		bool IReader.MoveToNextEntry()
		{
			return Original.MoveToNextEntry();
		}

		Task<bool> IReader.MoveToNextEntryAsync(CancellationToken cancellationToken)
		{
			return Original.MoveToNextEntryAsync(cancellationToken);
		}

		EntryStream IReader.OpenEntryStream()
		{
			return Original.OpenEntryStream();
		}

		Task<EntryStream> IReader.OpenEntryStreamAsync(CancellationToken cancellationToken)
		{
			return Original.OpenEntryStreamAsync(cancellationToken);
		}

		void IReader.WriteEntryTo(Stream writableStream)
		{
			Original.WriteEntryTo(writableStream);
		}

		Task IReader.WriteEntryToAsync(Stream writableStream, CancellationToken cancellationToken)
		{
			return Original.WriteEntryToAsync(writableStream, cancellationToken);
		}
	}
}
