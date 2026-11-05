# Design

Funk is a functional language:  everything in Funk gets converted to a function.

It allows you to compose and transform functions in order to converge to a result.

You can try all examples from the scratch pad.

##  Primitives

The simplest Funk program is a primitive, for instance:

```
42
```

Technically, a primitive is a parameter-less function returning a primitive constant.

See [primitives](primitives.md) for more details.

##  Function call

The next simplest program would be to invoke a function on primitives:

```
not(true)
```

This is invoking the function `sys.not`:  `sys` module always is implied.

##  Operators

Some functions have operator shorthands, for instance:

```
!true
```

translates to `sys.not(true)`.

See [operators](operators.md) for more details.

##  Function definitions

We can define functions:


```
let f(x) = x+1;

f(42)
```

(returns `43`)

We can also define a function that isn't meant to be resolved into something else, i.e. a sort of placeholder:

```
let f(x) = {}

f(5)
```

(returns `f(5)`)

##  Expression types

There are two main expressions in Funk:

Expression Type|Description|Examples
-|-|-
Function definition|Define a new function|`let x=2;`, `let f(x)=x+2`, etc.
Function evaluation|Evaluates a function|`x`, `f(42)`

## Contexts

There are three main contexts in Funk:

Context | Description
-|-
Scratch pad | Contains zero or many function definitions and one or zero function evaluation at the end
Function definition | Contains zero or many function definitions and one function evaluation at the end
Module | Contains only function definitions

