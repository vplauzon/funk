namespace Funk.Parsing
{
    public record BinaryArithmeticScript(
        BinaryArithmeticOperand Operand,
        ExpressionScript Left,
        ExpressionScript Right);
}