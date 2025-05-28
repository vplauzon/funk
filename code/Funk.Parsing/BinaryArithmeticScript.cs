namespace Funk.Parsing
{
    public record BinaryArithmeticScript(
        BinaryArithmeticOperand BinaryArithmeticOperand,
        ExpressionScript Left,
        ExpressionScript Right);
}