﻿namespace Funk.Parsing
{
    public record ExpressionScript(
        PrimitiveScript? Primitive = null,
        BinaryOperationScript? BinaryOperation = null,
        FunctionInvokeScript? FunctionInvoke = null,
        ExpressionScript? Parenthesis = null,
        ParameterAccessScript? ParameterAccess = null,
        IfScript? If = null);
}