using AsmResolver.PE;
using AsmResolver.PE.Exports;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace AssetRipper.Bindings.LLVM.Exports;

internal class Program
{
	static void Main(string[] args)
	{
		if (args.Length != 2)
		{
			Console.WriteLine("This program takes exactly two arguments: the path to the pe assembly and the path to the output file.");
			return;
		}

		try
		{
			Run(args[0], args[1]);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	private static void Run(string path, string outputPath)
	{
		if (TryLoadPEImage(path, out PEImage? peImage))
		{
			using StreamWriter writer = new(outputPath)
			{
				AutoFlush = true,
				NewLine = "\n",
			};
			writer.WriteLine("EXPORTS");
			List<string> exports = GetExportsForPE(peImage).ToList();
			exports.Sort(StringComparer.Ordinal);
			foreach (string export in exports)
			{
				writer.Write("    ");
				writer.WriteLine(export);
			}
		}
		else
		{
			Console.WriteLine($"Failed to load binary from path: {path}");
		}
	}

	private static IEnumerable<string> GetExportsForPE(PEImage image)
	{
		foreach (ExportedSymbol symbol in image.Exports?.Entries ?? [])
		{
			if (!symbol.IsByName)
			{
				continue;
			}
			if (!symbol.Name.StartsWith("LLVM", StringComparison.Ordinal))
			{
				continue;
			}
			yield return symbol.Name;
		}
	}

	private static bool TryLoadPEImage(string path, [NotNullWhen(true)] out PEImage? image)
	{
		try
		{
			image = PEImage.FromFile(path);
			return true;
		}
		catch
		{
			image = null;
			return false;
		}
	}
}
