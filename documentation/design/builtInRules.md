# Built-in rules

There are multiple built-in rules in *funk*.  All are case sensitive and most are in the `sys` namespace.

You can see [operators as rules](operatorsAsRules.md) (e.g. `1+2`).  Here we are going to look at non-operator rules.

Rule|Description|Example
-|-|-
`sys.toFloat`|Forces an expression to a float.  This works with integers, floats and division of integers (rational, see [arithmetics](arithmetics.md))|`sys.toFloat(1/2)` -> `0.5`
`sys.greatestCommonDivisor`|Find the greatest common denominator between two integers|`sys.greatestCommonDivisor(25, 15)` -> `5`
