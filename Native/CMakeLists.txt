cmake_minimum_required(VERSION 3.20)
project(LLVM)

include(GenerateExportHeader)

# Require C++17
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Specify where LLVM is installed
set(LLVM_ROOT "" CACHE PATH "Path to the root of the LLVM installation")
if (LLVM_ROOT STREQUAL "")
    message(FATAL_ERROR "LLVM_ROOT is not set. Please specify the path to the LLVM installation.")
endif()
if(NOT IS_DIRECTORY "${LLVM_ROOT}")
  message(FATAL_ERROR
    "Required directory does not exist:\n"
    "  ${LLVM_ROOT}"
  )
endif()
set(LLVM_INCLUDE_DIR "${LLVM_ROOT}/include")
set(LLVM_LIB_DIR "${LLVM_ROOT}/lib")

# Add include path for LLVM
include_directories(${LLVM_INCLUDE_DIR})

# Add include path for local headers
include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include)

# Add include path for generated export header
include_directories(${CMAKE_CURRENT_BINARY_DIR})

# Official LLVM static builds on Windows are almost always built with /MT, but CMake defaults to /MD
# Must be set BEFORE any targets are created
if(WIN32)
    set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
endif()

# Link time optimization can cause issues on macOS
if(APPLE)
    set(CMAKE_INTERPROCEDURAL_OPTIMIZATION OFF)
    add_compile_options(-fno-lto)
    add_link_options(-fno-lto)
endif()

# Add executable
add_library(LLVM SHARED src/Additions.cpp Exports.def)

if(WIN32)
    set_target_properties(LLVM PROPERTIES PREFIX "lib")
endif()

generate_export_header(LLVM
    EXPORT_MACRO_NAME "LLVM_API"
    EXPORT_FILE_NAME LLVM_export.h
)

# LLVM depends on ntdll on Windows
if(WIN32)
    target_link_libraries(LLVM PRIVATE ntdll)
endif()

# Determine the library extension based on platform
if(WIN32)
    set(LIB_EXT ".lib")
else()
    set(LIB_EXT ".a")
endif()

# Collect all library files in the directory
file(GLOB ALL_LIBS "${LLVM_LIB_DIR}/*${LIB_EXT}")

# Link against the import libraries
# Quote each path to safely handle spaces
foreach(LIB IN LISTS ALL_LIBS)
    target_link_libraries(LLVM PRIVATE "${LIB}")
endforeach()

