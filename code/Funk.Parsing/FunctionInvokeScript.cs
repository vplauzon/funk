namespace Funk.Parsing
{
    public record FunctionInvokeScript(
        string Namespace,
        string Name,
        IReadOnlyList<ExpressionScript> Parameters);
}
