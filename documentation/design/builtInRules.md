# Built-in rules

There are multiple built-in rules in *funk*.

You can see [operators as rules](operatorsAsRules.md) (e.g. `1+2`).  Here we are going to look at non-operator rules.

Rule|Description|Example
-|-|-
`sys.toFloat`|Forces an expression to a float.  This works with integers, floats and division of integers (rational, see [arithmetics](arithmetics.md))|`sys.toFloat(1/2)` -> `0.5`
`sys.param`|Select the parameter (base-0 index) of a function invocation|`sys.param(1/2, 0)` -> `1`
