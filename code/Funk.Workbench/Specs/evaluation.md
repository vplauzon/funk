# Script evaluation

The Workbench loads the .NET WebAssembly runtime in the browser and invokes
`WorkbenchJsInterop.EvaluateScript` when the user clicks **Evaluate** or presses
**Shift + Enter**. The editor contents are passed as a string, and the method
returns a JSON string serialized from `EvaluateScriptResult`.

The current method is a wiring stub: it returns a successful response with the
constant result `Hello world`. It does not yet call `FunkRuntime`. The browser
displays the returned result, and shows initialization or invocation errors in
the output panel.