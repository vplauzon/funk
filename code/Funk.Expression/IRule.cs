using Funk.Expression.Expressions;
using System.Collections.Immutable;

namespace Funk.Expression
{
    internal interface IRule
    {
        string Namespace { get; }

        string Name { get; }

        IImmutableList<string> ExpectedParameterNames { get; }

        ExpressionBase? Transform(IImmutableList<FunctionParameter> parameters);
    }
}