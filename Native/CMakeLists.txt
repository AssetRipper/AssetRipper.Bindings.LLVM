cmake_minimum_required(VERSION 3.20)
project(libLLVM)

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
endif()

# Add executable
add_library(libLLVM SHARED src/Additions.cpp Exports.def)

generate_export_header(libLLVM
    EXPORT_MACRO_NAME "LLVM_API"
    EXPORT_FILE_NAME LLVM_export.h
)

# LLVM depends on ntdll on Windows
if(WIN32)
    target_link_libraries(libLLVM PRIVATE ntdll)
endif()

# Determine the library extension based on platform
if(WIN32)
    set(LIB_EXT ".lib")
else()
    set(LIB_EXT ".a")
endif()

# Collect all library files in the directory
file(GLOB ALL_LIBS "${LLVM_LIB_DIR}/*${LIB_EXT}")

# Remove the ones we don't want
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/libclang${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/libiomp5md${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/liblldb${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/libomp${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/LLVM-C${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/LTO${LIB_EXT}")
list(REMOVE_ITEM ALL_LIBS "${LLVM_LIB_DIR}/Remarks${LIB_EXT}")

# Link against the import libraries
# Quote each path to safely handle spaces
foreach(LIB IN LISTS ALL_LIBS)
    target_link_libraries(libLLVM PRIVATE "${LIB}")
endforeach()

