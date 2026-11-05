# Design

Funk is a functional programming language:  everything in Funk is represented by a function.

It allows you to compose and transform functions in order to converge to a result.

You can try all examples from the scratch pad.

##  Litterals

The simplest Funk program is a litteral, for instance:

```
42
```

Technically, a litteral is a constant primitive and a primitive is a parameter-less function.

See [primitives](primitives.md) for more details.

##  Function call

The next simplest program would be to invoke a function on primitives:

```
not(true)
```

This is invoking the function `sys.not`:  `sys` module always is implied.

##  Operators

Some functions have operator syntax as syntactic sugar, for instance:

```
!true
```

translates to `sys.not(true)`.

See [operators](operators.md) for more details.

##  Function definitions

We can define functions.  The simplest would be a parameterless function returning a primitive:

```
let x = 42;

x
```

(returns `42`)

Something slithly more complex and useful:

```
let f(x) = x+1;

f(42)
```

(returns `43`)

We can also define a function that isn't meant to be resolved into something else, i.e. a symbolic function:

```
let f(x) = {}

f(5)
```

(returns `f(5)`)

See [function names](functionNames.md) for more details.

## Conditional expressions

A short conditional statement:

```
let x=3;

if(x==2, "Yes", "No")
```

(returns `"No"`)

The long (equivalent) form:

```
let x=3;

if(x==2)
{
    "Yes"
}
else
{
    "No"
}
```

(still returns `"No"`)

That form also allows "else if" chains:

```
let x=3;

if(x==2)
{
    "Yes"
}
else if(x==1)
{
    "Maybe"
}
else
{
    "No"
}
```

(still returns `"No"`)


#   TODO:  continue here

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

