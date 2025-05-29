# Operators as rules

A lot of operators are translated into rule evocation.  For instance, `1+2` translates to `add(1,2)` (which then translates to `3`).  Here is a list of operators and how they translate:

Operator|Example|Translation
-|-|-
==|`2==1`|`sys.equals(2, 1)`
+|`1+2`|`sys.add(1,2)`
-|`1-2`|`sys.substract(1,2)`
*|`1*2`|`sys.multiply(1,2)`
/|`1/2`|`sys.div(1,2)`
^|`3^2`|`sys.power(3,2)`

See [arithmetics](arithmetics.md) to go deeper.