namespace Funk.Parsing
{
    public record ChainedIfElseScript(
        ExpressionScript Condition,
        ExpressionScript ThenExpression,
        IReadOnlyList<ElseIfClauseScript> ElseIf,
        ExpressionScript? ElseExpression);
}
