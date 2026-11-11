#	Runtime specs

This technical specs is realizing the functional specs defined in the folder `documentation` at
the root of the repository.

##	Non functional requirements

A few extra goals:

*	We want to run multiple scripts in parallel and sequentially
*	We do not want to start from scratch every time:  we want to cache as much as possible

##	Main components

###	FunkRuntime

Main entry point for the runtime and acts as a singleton as it holds the cache for many runs.

Its main public method allows to run a script and returns the computed value.

### ModuleCache

Caches modules that have been loaded.  It provides methods to JIT evaluate / simplify / specialize
function declaration in a multi-thread safe way.

### ModuleLoader

Interface with the storage API to load modules.

### ScriptRunner

Runs either a script or a function evaluation.  Can be created hierarchically.