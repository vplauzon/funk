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

Here `x` is a parameter and take precedence to a function `x` defined in another rule if there was such a rule.

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
let add(vector(*) a, vector(*) b) = vector(a.x+b.x, a.y+b.y, a.z+b.z);
```

Here that rule matches the `add` function only when both parameters are vectors.

We could be even more sophisticated in pattern matching with:

```
let add(vector(*) a, vector(*) b) if (a==b) = 2*vector(x,y,z);
```

Here the rule applies only when the two parameters have the same value.

Rules do not always simplify an expression.  Sometimes it simplifies, sometimes it makes an expression more complex (e.g. `let f(x) = x+2;`) and sometimes it leaves the complexity untouched:

```
let sum(a,b) = sum(b,a);
```

This rule is stating a sum is commutative and could allow other rules to fire.

## Parameter list

Func allows you to access the parameter of a function by name, e.g.:

```
vector(1,2,3).x
```

In order to do that, the parameter list must be canonical across all the rules pertaining to that function:

* The list of parameters must always have the same length
* The parameter names must always be the same

In that sense, in *func*, a function invocation is akeen to a read-only class against which transformation rules are defined.


See [operators as rules](operatorsAsRules.md), [built-in rules](builtInRules.md) and [rule expension](ruleExpension.md) to get deeper.

