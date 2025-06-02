using Funk.Expression.Expressions;
using System.Collections.Immutable;

namespace Funk.Expression
{
    internal interface IRule
    {
        string Namespace { get; }

        string Name { get; }

        IImmutableList<string> ParameterNames { get; }

        ExpressionBase? Transform(IImmutableList<ExpressionBase> parameters);
    }
}