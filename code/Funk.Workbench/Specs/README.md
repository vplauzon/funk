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
3. **Result** — a read-only output panel with placeholder evaluation output.

The explorer and result panes can be resized horizontally using their dividers.
The dividers also support keyboard resizing when focused. On narrow screens the
panes stack vertically and the dividers are hidden.

## Interactions

- The tree folders expand and collapse using native disclosure controls.
- The editor supports typing, line-number updates, and inserting two spaces with Tab.
- **Evaluate** button (shortcut to **Shift + Enter**) triggers the current placeholder evaluation.
  The result is not yet computed from the editor contents; it currently displays
  hard-coded output and updates the last-run time.

## Technical scope

The Workbench is an ASP.NET Core web project targeting .NET 10. ASP.NET Core
serves the static files from `wwwroot`; the interface and current interactions
are implemented in HTML, CSS, and vanilla JavaScript. Editor integration, backend
APIs, persistence, and Funk evaluation can be added incrementally as the
Workbench grows.

The launch profile uses `http://localhost:5159`. In production, the intended
hosting environment is Azure Container Apps, which terminates HTTPS in front of
the container.