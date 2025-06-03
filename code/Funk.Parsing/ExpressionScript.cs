namespace Funk.Parsing
{
    public record ExpressionScript(
        PrimitiveScript? Primitive = null,
        BinaryArithmeticScript? ArithmeticBinary = null,
        FunctionInvokeScript? FunctionInvoke = null,
        ExpressionScript? Parenthesis = null,
        ParameterAccessScript? ParameterAccess = null);
}