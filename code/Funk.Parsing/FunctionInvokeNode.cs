namespace Funk.Parsing
{
    public record FunctionInvokeNode(
        string Namespace,
        string Name,
        IReadOnlyList<FunctionEvaluationStatementNode> Parameters);
}