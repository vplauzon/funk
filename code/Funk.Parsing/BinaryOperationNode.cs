namespace Funk.Parsing
{
    public record BinaryOperationNode(
        BinaryOperator Operator,
        FunctionEvaluationStatementNode Left,
        FunctionEvaluationStatementNode Right);
}