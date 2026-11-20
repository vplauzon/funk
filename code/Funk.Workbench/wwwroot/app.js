const editor = document.querySelector("#code-editor");
const lineNumbers = document.querySelector("#line-numbers");
const cursorPosition = document.querySelector("#cursor-position");
const filenameLabel = document.querySelector("#active-filename");
const output = document.querySelector("#result-output");
const runTime = document.querySelector("#run-time");
const evaluateButton = document.querySelector("#evaluate");
const runBadge = document.querySelector(".run-badge");
const workspace = document.querySelector("#workspace");

let evaluateScript = null;
evaluateButton.disabled = true;
runBadge.innerHTML = '<span class="run-dot"></span> LOADING';

const samples = {
  "main": `// A tiny Funk program\nlet greeting = "Hello, Funk!"\nlet answer = 40 + 2\n\nanswer`,
  "math": `// Functions are values\nlet double = x => x * 2\nlet square = x => x * x\n\ndouble(6) + square(3)`,
  "lists": `// Working with collections\nlet numbers = [1, 2, 3, 4, 5]\nlet doubled = numbers.map(x => x * 2)\n\ndoubled`,
  "records": `// A simple record\nlet user = {\n  name: "Ada",\n  language: "Funk"\n}\n\nuser.name`,
  "notes": `// Scratchpad\n// Try a small expression here\n\n(8 * 5) + 2`
};

function updateLineNumbers() {
  const count = editor.value.split("\n").length;
  lineNumbers.textContent = Array.from({ length: count }, (_, index) => index + 1).join("\n");
  lineNumbers.scrollTop = editor.scrollTop;
}

function updateCursorPosition() {
  const beforeCursor = editor.value.slice(0, editor.selectionStart);
  const lines = beforeCursor.split("\n");
  cursorPosition.textContent = `Ln ${lines.length}, Col ${lines.at(-1).length + 1}`;
}

function updateRunTime() {
  runTime.textContent = new Date().toLocaleTimeString([], {
    hour: "numeric",
    minute: "2-digit",
    second: "2-digit"
  });
}

function showRunStatus(status) {
  runBadge.innerHTML = `<span class="run-dot"></span> ${status}`;
}

function evaluate() {
  if (evaluateScript === null) {
    return;
  }

  showRunStatus("RUNNING");
  try {
    const response = JSON.parse(evaluateScript(editor.value));
    output.textContent = response.ok ? response.result : response.error;
    showRunStatus(response.ok ? "READY" : "ERROR");
  } catch (error) {
    output.textContent = error instanceof Error ? error.message : String(error);
    showRunStatus("ERROR");
  }
  updateRunTime();
}

async function initializeRuntime() {
  try {
    const { dotnet } = await import("/_framework/dotnet.js");
    const { getAssemblyExports, getConfig, runMain } = await dotnet.create();
    const config = getConfig();
    const exports = await getAssemblyExports(config.mainAssemblyName);
    await runMain();

    const workbenchApi = exports.Funk.Workbench.JsInteropLayer.WorkbenchJsInterop;
    evaluateScript = workbenchApi.EvaluateScript;
    if (typeof evaluateScript !== "function") {
      throw new Error("The WebAssembly evaluation method is unavailable.");
    }
    evaluateButton.disabled = false;
    output.textContent = "Ready to evaluate.";
    showRunStatus("READY");
  } catch (error) {
    output.textContent = error instanceof Error ? error.message : String(error);
    showRunStatus("ERROR");
  }
}

editor.value = samples["main"];
updateLineNumbers();
updateCursorPosition();
editor.addEventListener("input", updateLineNumbers);
editor.addEventListener("scroll", () => { lineNumbers.scrollTop = editor.scrollTop; });
editor.addEventListener("click", updateCursorPosition);
editor.addEventListener("keyup", updateCursorPosition);
editor.addEventListener("keydown", event => {
  if (event.key === "Enter" && event.shiftKey) {
    event.preventDefault();
    evaluate();
  }
  if (event.key === "Tab") {
    event.preventDefault();
    const start = editor.selectionStart;
    const end = editor.selectionEnd;
    editor.setRangeText("  ", start, end, "end");
    updateLineNumbers();
    updateCursorPosition();
  }
});
evaluateButton.addEventListener("click", evaluate);
initializeRuntime();

document.querySelectorAll("[data-file]").forEach(button => {
  button.addEventListener("click", () => {
    document.querySelector(".file-item.is-active")?.classList.remove("is-active");
    document.querySelector(".file-item[aria-current='page']")?.removeAttribute("aria-current");
    button.classList.add("is-active");
    button.setAttribute("aria-current", "page");
    filenameLabel.textContent = button.dataset.file;
    editor.value = samples[button.dataset.file];
    editor.scrollTop = 0;
    editor.selectionStart = 0;
    editor.selectionEnd = 0;
    updateLineNumbers();
    updateCursorPosition();
    editor.focus();
  });
});

function resizePane(splitter, clientX) {
  const bounds = workspace.getBoundingClientRect();
  const kind = splitter.dataset.splitter;
  if (kind === "explorer") {
    const width = Math.max(190, Math.min(420, clientX - bounds.left - 19));
    workspace.style.setProperty("--explorer-width", `${width}px`);
    splitter.setAttribute("aria-valuenow", String(Math.round(width)));
  } else {
    const width = Math.max(240, Math.min(560, bounds.right - clientX - 19));
    workspace.style.setProperty("--result-width", `${width}px`);
    splitter.setAttribute("aria-valuenow", String(Math.round(width)));
  }
}

document.querySelectorAll(".splitter").forEach(splitter => {
  splitter.addEventListener("pointerdown", event => {
    splitter.setPointerCapture(event.pointerId);
    splitter.classList.add("is-dragging");
    resizePane(splitter, event.clientX);
  });
  splitter.addEventListener("pointermove", event => {
    if (splitter.hasPointerCapture(event.pointerId)) resizePane(splitter, event.clientX);
  });
  const stopDragging = () => splitter.classList.remove("is-dragging");
  splitter.addEventListener("pointerup", stopDragging);
  splitter.addEventListener("pointercancel", stopDragging);
  splitter.addEventListener("keydown", event => {
    if (event.key !== "ArrowLeft" && event.key !== "ArrowRight") return;
    event.preventDefault();
    const bounds = workspace.getBoundingClientRect();
    const delta = event.key === "ArrowLeft" ? -16 : 16;
    const coordinate = splitter.dataset.splitter === "explorer"
      ? bounds.left + parseFloat(getComputedStyle(workspace).getPropertyValue("--explorer-width")) + delta
      : bounds.right - parseFloat(getComputedStyle(workspace).getPropertyValue("--result-width")) - delta;
    resizePane(splitter, coordinate);
  });
});
