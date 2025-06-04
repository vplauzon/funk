# Operators as rules

A lot of operators are translated into rule evocation.  For instance, `1+2` translates to `add(1,2)` (which then translates to `3`).  Here is a list of operators and how they translate:

Operator|Example|Translation
-|-|-
+|`1+2`|`sys.add(1,2)`
-|`1-2`|`sys.substract(1,2)`
*|`1*2`|`sys.product(1,2)`
/|`1/2`|`sys.division(1,2)`
^|`3^2`|`sys.power(3,2)`
==|`2==1`|`sys.equals(2, 1)`
!=|`2!=1`|`sys.notEquals(2, 1)`
<=|`2<=1`|`sys.lesserOrEquals(2, 1)`
<|`2<1`|`sys.lesser(2, 1)`
\>=|`2>=1`|`sys.greaterOrEquals(2, 1)`
\>|`2>1`|`sys.greater(2, 1)`
&&|`true && true`|`sys.and(true, true)`
\|\||`true \|\| true`|`sys.or(true, true)`

See [arithmetics](arithmetics.md) to go deeper.