# Rule expension

An expression goes through different rules to transform itself.

For instance, the following expression:

```
2+3
```

This is [understood](operatorsAsRules.md) as the following function evocation:

```
add(2,3)
```

Which is then simplified into:

```
5
```

An expression can be a function definition, for instance:

```
def f(a, b, c=12, d=a+b)
```

A function definition is simply the name of the function and the list of parameters.  Parameters can have a default value with the constraint that a parameter without a default value cannot appear after one with a default value.
