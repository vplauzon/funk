namespace Funk.Parsing
{
    public record FunctionInvokeScript(
        string Namespace,
        string Name,
        ExpressionScript[] Parameters);
}