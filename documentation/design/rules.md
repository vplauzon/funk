# Rules

A rule can use other rules:

```
let y = a+b;
```

(assuming `a` & `b` has been defined)

A rule can use parameter:

```
let f(x) = x+2;
```

Here `x` is a mute parameter, i.e. it overrides the function `x` defined above.

A rule can have a more complex body:

```
let f(x)
{
    let a=2;

    return x+a;
};
```

In that case, the body can define other rules (here `a` is defined) and *return*.

More than one parameter can be used and some can have *default* values:

```
let g(x, y=2) = x+y;
```

A rule can be defined as a placeholder:

```
let vector(x,y,z);
```

Here `vector` doesn't transform into anything but can be used in other expression.

We can then use more complex pattern matching with other rules:

```
let add(vector(x1,y1,z1), vector(x2,y2,z2)) = vector(x1+x2,y1+y2,z1+z2);
```

Here that rule matches the `add` function only when both parameters are vectors.

We could even be more sophisticated in pattern matching with:

```
let add(vector(x,y,z), vector(x,y,z)) = 2*vector(x,y,z);
```

Here the rule applies only when the two parameters have the same value.

Rules do not always simplify an expression.  Sometimes it simplifies, sometimes it makes an expression more complex (e.g. `let f(x) = x+2;`) and sometimes it leaves the complexity untouched:

```
let sum(a,b) = sum(b,a);
```

This rule is stating a sum is commutative and could allow other rules to fire.

See [operators as rules](operatorsAsRules.md), [built-in rules](builtInRules.md) and [rule expension](ruleExpension.md) to get deeper.

