# Generated Code

## C Wrapper

### Structs

### Unions

### Enums

### Classes

These are represented as structs containing a pointer.

### Functions

C++ function calls are wrapped in a try catch block to allow errors to be propogated to managed land.

### Fields

Generated functions for fields return a memory address for the field.

For instance fields, the generated function returns null if the object instance is null.

## C# Reconstruction

In the generated C# code, we attempt to maintain as much of the original C++ structure as possible.

## Limitations

* No templates. Anything that has them in the signature cannot be used.
