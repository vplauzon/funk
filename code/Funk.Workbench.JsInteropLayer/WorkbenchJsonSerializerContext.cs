using System.Text.Json.Serialization;

namespace Funk.Workbench.JsInteropLayer;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(EvaluateScriptResult))]
internal partial class WorkbenchJsonSerializerContext : JsonSerializerContext
{
}