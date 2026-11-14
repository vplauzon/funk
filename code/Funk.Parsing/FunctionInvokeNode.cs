namespace Funk.Parsing
{
    public record FunctionInvokeNode(
        string Name,
        IReadOnlyList<FunctionEvaluationStatementNode> Parameters);
}