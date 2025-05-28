using Funk.Expression.Expressions;

namespace Funk.Expression
{
    internal interface IRule
    {
        string Namespace { get; }

        string Name { get; }

        ExpressionBase? Transform(FunctionInvokeExpression expression);
    }
}