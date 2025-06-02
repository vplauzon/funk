# Funk guidelines

These guidelines are meant for AI agents.

## Syntax preferences

We want code lines to be shorter than 100 characters.

When a line is longer than 100 characters, we move method parameters to its own line, e.g.:

```
    var myVariable = myObject.InvokingMethod("My first parameter", "My second parameter", 65200, "My third parameter");
```

becomes

```
    var myVariable = myObject.InvokingMethod(
        "My first parameter",
        "My second parameter",
        65200,
        "My third parameter");
```