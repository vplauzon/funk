# Funk Workbench Specs

Funk Workbench is a single-page browser workspace for exploring and evaluating
Funk modules. This document describes the current Workbench so its behavior and
scope are clear to contributors and coding agents.

## Layout

The page has a compact top bar, a three-pane workspace, and a bottom status bar.
The workspace panes are arranged from left to right:

1. **Module Explorer** — a collapsible tree of files and folders.
   The tree uses module names without file extensions. Its content is currently hard-coded;
   no API is connected yet.
2. **Editor** — a large, editable text area with line numbers, a Scratch Pad
   tab, cursor position, and basic indentation support.
3. **Result** — a read-only output panel with evaluation output.

The explorer and result panes can be resized horizontally using their dividers.
The dividers also support keyboard resizing when focused. On narrow screens the
panes stack vertically and the dividers are hidden.

## Interactions

- The tree folders expand and collapse using native disclosure controls.
- The editor supports typing, line-number updates, and inserting two spaces with Tab.
- **Evaluate** button (shortcut to **Shift + Enter**) invokes .NET WebAssembly.
  See [evaluation](evaluation.md) for details.

## Technical scope

The Workbench is an ASP.NET Core web project targeting .NET 10. ASP.NET Core
serves static files from `wwwroot` and the referenced .NET WebAssembly project.
The interface is implemented in HTML, CSS, and vanilla JavaScript. Evaluation
calls the WebAssembly interop layer in the browser. Its current response is a
placeholder; it does not yet evaluate Funk scripts.

Use the `http` launch profile at `http://localhost:5159` for local development;
it does not require a local HTTPS certificate. Visual Studio Hot Reload may
separately prompt for an IIS Express certificate. In production, Azure
Container Apps terminates HTTPS in front of the container.

## Script evaluation

The Workbench loads the .NET WebAssembly runtime in the browser and invokes
`WorkbenchJsInterop.EvaluateScript` when the user clicks **Evaluate** or presses
**Shift + Enter**. The editor contents are passed as a string, and the method
returns a JSON string serialized from `EvaluateScriptResult`.

The current method is a wiring stub: it returns a successful response with the
constant result `Hello world`. It does not yet call `FunkRuntime`. The browser
displays the returned result, and shows initialization or invocation errors in
the output panel.