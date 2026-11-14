#	Runtime specs

This technical specs is realizing the functional specs defined in the folder `documentation` at
the root of the repository.

##	Non functional requirements

We want to cache scripts and their core representations between script execution.

This way, subsequent script execution with little changes should execute fast.

For that we need to keep track of dependencies so we can invalidate scripts.

##	Main components

###	FunkRuntime

Main entry point for the runtime, owns `ModuleCache`.

Its main public method allows to run a script and returns the computed value.

### ModuleCache

Caches modules that have been loaded, owns `ModuleLoader`.

### ModuleLoader

Interface with the storage API to load modules.

### FunctionLibrary

Represents a function library.  This could be a module or a function definition or a function
definition within a function definition.

It can have a parent `FunctionLibrary` and can also reference modules.

It keeps resolving its function definitions in a JIT manner.

### FunctionEvaluationResolver

Contains a function evaluation which it can resolve.

It also has a parent `FunctionLibrary`.
