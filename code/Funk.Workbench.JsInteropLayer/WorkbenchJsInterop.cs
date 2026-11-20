using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json;

namespace Funk.Workbench.JsInteropLayer;

public static partial class WorkbenchJsInterop
{
    [JSExport]
    [SupportedOSPlatform("browser")]
    public static string EvaluateScript(string script)
    {
        var response = new EvaluateScriptResult
        {
            Ok = true,
            Result = "Hello world"
        };

        return JsonSerializer.Serialize(
            response,
            WorkbenchJsonSerializerContext.Default.EvaluateScriptResult);
    }
}