# Operators as rules

A lot of operators are translated into rule evocation.  For instance, `1+2` translates to `add(1,2)` (which then translates to `3`).  Here is a list of operators and how they translate:

Operator|Example|Translation
-|-|-
==|`2==1`|`equals(2, 1)`
+|`1+2`|`add(1,2)`
-|`1-2`|`substract(1,2)`
*|`1*2`|`multiply(1,2)`
/|`1/2`|`div(1,2)`

