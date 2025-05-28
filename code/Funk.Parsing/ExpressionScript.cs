namespace Funk.Parsing
{
    public record ExpressionScript(
        PrimitiveScript? Primitive,
        BinaryArithmeticScript ArithmeticBinary);
}