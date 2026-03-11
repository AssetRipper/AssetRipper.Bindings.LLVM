# Naming Convention

To prevent conflicts and maintain necessary information, a naming scheme is necessary.

Original C++: Namespace.Sub_Namespace.TypeName.Member

Generated C: Prefix__Namespace__Sub_1Namespace__TypeName__Member_i_h00000000

Underscores must be followed by another character.

## Prefix

To prevent conflict with symbols in the global namespace, a prefix is added to all symbols.

## Periods

A period (or `::`) in the original C++ maps to two consecutive underscores.

## Underscores in the original

If the original contained a sequence of underscores, those are represented by an underscore followed by a digit. In the unlikely event that the original had more than 9 consecutive underscores, they are separated into groups of 9. For example, 20 underscores would be `_9_9_2`.

## Overloaded members

If `_h` is encountered in the generated C symbol, anything that follows is considered an implementation detail to handle multiple potential overloads.

## Instance and static members

`_i` and `_s` represent the C# concepts of instance and static, respectively.
