# Operators

A lot of operators are translated into function evaluation.  For instance, `1+2` translates to `sys.add(1,2)` (which evaluates to `3`).

>>Note that operators gets translated to an unbound function evaluation.  Depending on the [type](types.md) of the parameters, it might bind to different function.  For instance, e.g. `1+2` translates to `add(1, 2)` which binds to `sys.add(integer, integer)`.  But `v1+v2` would also translates to `add(v1, v2)` and since `v1` & `v2` are, let's say `myVector` symbolic function, then it might bind to `myModule.add(myVector, myVector)`.

>>Also note that operators are translated to unbound functions in arithmetic order.  For instance, `1+2*3` is translated to `add(1, product(2,3))` while `1*2+3` would be translated to `add(product(1,2), 3).

## Arithmetic operators

Operator|Example|Translation
-|-|-
+|`1+2`|`add(1,2)`
\-|`1-2`|`substract(1,2)`
\- (unary)|`-42`|`negation(42)`
*|`1*2`|`product(1,2)`
/|`1/2`|`div(1,2)`
^|`3^2`|`pow(3,2)`

See [arithmetics](arithmetics.md) to go deeper.

## Logical operators

Operator|Example|Translation
-|-|-
==|`2==1`|`equals(2, 1)`
!=|`2!=1`|`notEquals(2, 1)`
<=|`2<=1`|`lesserOrEquals(2, 1)`
<|`2<1`|`lesser(2, 1)`
\>=|`2>=1`|`greaterOrEquals(2, 1)`
\>|`2>1`|`greater(2, 1)`
!|`!true`|`not(true)`
&&|`true && true`|`and(true, true)`
\|\||`true \|\| true`|`or(true, true)`
