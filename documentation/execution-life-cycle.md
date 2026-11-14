#   Execution life cycle

We will take a small Funk program and see how it gets executed.

```
let f(x) = x+3;

let g(y) = y+4;

f(6-5)
```

This will return `4`.

##  Parsing

First this script will be parsed into a Abstract Syntax Tree (AST).

The AST represents the exact script.  For instance the addition and substraction operators aren't converted into functions yet.

##  Convert to core representation

AST is then converted into a core representation.

This is done in a lazy way to avoid converting function declarations that aren't used.  For instance, in our script, the function `g(y)` is never used.  The initial conversion is therefore superficial.

In-memory, this results in something like this:

```
FUNCTION DECLARATION (AST(let f(x) = x+3))
FUNCTION DECLARATION (AST(let g(y) = y+4))

EVALUATION (AST(f(6-5)))
```

##  Evaluation resolution

It is now time to try to resolve the evaluation and recursively resolve function declaration on a need-to (JIT) basis.

This starts with a deep conversion of the evaluation into core representation:

```
EVALUATION (UNBOUND(f)(substract(6,5)))
```

Then we loop between two tasks:

*   Function evaluation
*   Function binding

This is done until nothing change.

### Function evaluation

In our case we catch the `sys.substract`:

```
EVALUATION (UNBOUND(f)(1))
```

### Function binding

We can then bind `f` with the `f` that is defined in the same context:

```
FUNCTION DECLARATION (DECLARE(f, ['x'], sys.add(x, 3)))
FUNCTION DECLARATION (AST(let g(y) = y+4))

EVALUATION (BOUND(f)(1))
```

##  Looping

The function declaration passes through that loop itself.  In our case, it stops quickly since `x` is a parameter and the addition cannot be resolved.

The loop continue for the evaluation though.  `f` is evaluated:

```
EVALUATION (sys.add(1, 3))
```

And `sys.add` is evaluated:

```
EVALUATION (4)
```

At this point, nothing more changes and `4` is returned.