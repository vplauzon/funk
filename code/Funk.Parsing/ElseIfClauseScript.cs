namespace Funk.Parsing
{
    public record ElseIfClauseScript(
        ExpressionScript Condition,
        ExpressionScript Expression);
}
