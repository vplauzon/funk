namespace Funk.Workbench.JsInteropLayer;

internal sealed class EvaluateScriptResult
{
    public bool Ok { get; init; }
    public string Result { get; init; } = string.Empty;
}