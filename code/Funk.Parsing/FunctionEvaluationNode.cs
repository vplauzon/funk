namespace Funk.Parsing
{
    public record FunctionEvaluationNode(
        PrimitiveNode? Primitive = null,
        BinaryOperationNode? BinaryOperation = null,
        FunctionInvokeNode? FunctionInvoke = null,
        FunctionEvaluationStatementNode? Parenthesis = null,
        ParameterAccessNode? ParameterAccess = null,
        IfNode? If = null);
}