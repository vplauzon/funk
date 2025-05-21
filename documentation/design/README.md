# Design

Funk is a functional language.  It allows to compose and transform functions in order to converge to a result.

The fundamentals of Funk are expressions and rules.  Rules allows expressions to be transformed and converge into an answer.

Rules can be defined anywhere in any scope.  They apply only in the scope they are defined.

## Expressions

An expression can be a primitive, a list or a function reference.

A primitive can be a boolean, an integer, a float or a string.  You can see that as the leaves of a tree.

A list is a list of expressions.  Elements can be homogeneous, e.g. `{1,2,3}`, or heterogeneous, e.g. `{1, "hello", true}`.

A function reference can be with no parameters, e.g. `f`, partial parameter bindings, e.g. `f(1)` or complete parameter bindings, e.g. `f(1,2)`.  Here we assume `f` has two parameters in the example of the previous sentence.

## Rules

A rule essential extend what expressions can be and how expressions transform into other expressions.  They are defined with the keyword `let`.  A simple rule looks like a variable definition:

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
