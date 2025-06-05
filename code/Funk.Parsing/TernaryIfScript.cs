namespace Funk.Parsing
{
    public record TernaryIfScript(
        ExpressionScript Condition,
        ExpressionScript TrueExpression,
        ExpressionScript FalseExpression);
}