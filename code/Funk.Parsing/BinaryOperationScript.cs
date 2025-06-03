namespace Funk.Parsing
{
    public record BinaryOperationScript(
        BinaryOperator Operator,
        ExpressionScript Left,
        ExpressionScript Right);
}