# Types

In Funk, a type is a subset of accepted functions.

For instance, an `integer` is a subset of all possible functions, i.e. only the ones with no parameter returning an integer.

[Primitives](primitives.md) all have a corresponding type.  Those are the fundamental blocks for types.

## Type disjunction

We can build new types by disjunction.  For instance:

```
integer | real
```

represents the subset of any functions being an integer or a real.

The `sys` module define the primitive types but also define `rational` as a symbolic function:

```
let rational(integer a, integer b) = {}
```

and then `numerical` is defined as:

```
type numerical = integer | real | rational;
```

## Type definition

We can define new types.  There are 2 main scenarios for type definitions:

1.  symbolic function value
2.  function

In the first scenario, we want to pass a symbolic function value around (e.g. `rational(2,3)`).

In the second scenario, we want to pass a function around so we are interested in defining the function's *signature*:  its parameters and return type.

### Symbolic function value

This is done by simply naming the function:

```
let f(rational x) = x+1;
```

Here `rational` is a symbolic function.

### Function

We can define an expected function signature as follow:

```
let f(integer (integer, integer) g) = g(1, 2);
```

The function `g` here is defined as taking two integers in and returning an integer.  We can therefore deduce that `f` returns an integer.

The function could also return a function:

```
let f((integer (integer)) (integer, integer) g) = (g(1, 2))(3);
```

Here function `g` is defined as taking two integers in and returning a function itself taking one integer in and returning an integer.

## Standalone type vs inline

A type can be defined with a name in its scope, for example:

```
type myFunction = integer (integer, integer);
```

This is useful for reusable types, making function definition lighter:

```
let f(myFunction g) = g(1, 2);
```

##  Infering return types

In Funk return types are always inferred.

For instance:

```
let f(integer x) = x+1;
```

will be inferred to return an `integer` since `sys.add(integer x, integer y)` returns an `integer`.

Sometimes the return type will be impossible to infer and will be undetermined.

## Undefined parameter type

A function such as:

```
let f(x) = x+1;
```

doesn't define the type of the input `x`.  This makes `x` generic:  i.e. it can take on any type.

When the function is used, for instance:

```
f(1)
```

Then a specialization of that function is defined under the hood:

```
let f(integer x) = x+1;
```

and is used to resolve `f(1)`.

If an incompatible type is used, for instance:

```
f("Hello")
```

The specialization will fail since `"Hello" + 1` isn't defined.