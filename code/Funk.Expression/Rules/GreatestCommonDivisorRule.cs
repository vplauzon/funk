namespace Funk.Expression.Rules
{
    using Funk.Expression.Expressions;
    using Funk.Parsing;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    internal class GreatestCommonDivisorRule : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => "greatestCommonDivisor";

        IImmutableList<string> IRule.ParameterNames => BinaryOperationHelper.ParameterNames;

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  We have 2 integers
            if (left is PrimitiveExpression leftP
                && leftP.PrimitiveCategory == PrimitiveCategory.Integer
                && right is PrimitiveExpression rightP
                && rightP.PrimitiveCategory == PrimitiveCategory.Integer)
            {
                var l = leftP.ToInteger();
                var r = rightP.ToInteger();
                var gcd = (int)BigInteger.GreatestCommonDivisor(l, r);

                return PrimitiveExpression.Create(gcd);
            }

            return null;
        }
    }
}
