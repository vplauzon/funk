namespace Funk.Parsing
{
    public record IfThenNode(FunctionEvaluationStatementNode Condition, FunctionEvaluationStatementNode ThenExpression);
}