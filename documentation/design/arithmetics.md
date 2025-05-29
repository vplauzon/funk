# Arithmetics

Arithmetics in *funk* is quite similar as other programming language.  For instance, `1+2` will transform into `3`.

Where the similarities end is with the handling of integer division.  In *funk*, `1/2` doesn't transform into `0.5` or `0`.  It stays as `1/2` (or `div(1,2)`).  *funk* handles this as a rational number.

*funk* implements greatest numerator simplification.  In that sense, `div(2, 4)` transforms into `div(1,2)`.  Similarly, `1/2+1/4` transforms into `div(3,4)`.

To force a rational number into a float, use the `toFloat` built-in rule:  `toFloat(1/2)` transforms into 0.5.