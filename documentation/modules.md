#   Modules

A module is a separate script, saved in a file, where functions and types can be defined.

It has the same syntax as an ad hoc script with the following exceptions:

*   There is no "final function evaluation" in a module, just type and function definition
*   Types & functions can be marked as `private` so they are not visible for script importing it

## Private declaration

A function can be marked as `private`:

```
private let f(x) = x+1;
```

Similarly, a type can be marked as `private`:

```
private type myType = integer (integer);
```

##  Using modules

A module can be used by *importing* it:

```
import myModule;
```

The following modules are implicitly imported in ad hoc scripts
and other modules:

```
import sys;
import sys.arithmetic;
```
