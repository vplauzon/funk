# Design

Funk is a functional language.  It allows you to compose and transform functions in order to converge to a result.

The fundamentals of Funk are expressions and rules.  Rules allows expressions to be transformed and converge into an answer.

Rules can be defined anywhere in any scope.  They apply only in the scope they are defined.

## Expressions

An expression can be a primitive, a list or a function reference.

A primitive can be a boolean, an integer, a float or a string.  You can see that as the leaves of a tree.

A list is a list of expressions.  Elements can be homogeneous, e.g. `{1,2,3}`, or heterogeneous, e.g. `{1, "hello", true}`.  A list can be defined explicitly, e.g. `{1,2,3,4}`, as a range, e.g. `{1.. 4}` or as a ??? from another list, e.g. `{x in {1,2,3,4} | x^2}`.

A function reference can be with no parameters, e.g. `f`, partial or complete parameter bindings, e.g. `f(1)`.  A function reference can also call parameters by name, e.g. `g(x=1, z=3)`.

## Rules

A rule extends what expressions can be and how expressions transform into other expressions.  They are defined with the keyword `let`.

A simple rule looks like a variable definition:

```
let x = 2;
```

This can be used in an expression.  For instance:

```
x+1
```

will transform in the primitive expression `3`.  So will

```
x()+1
```

See the [rules page](rules.md) for more details about rules.
