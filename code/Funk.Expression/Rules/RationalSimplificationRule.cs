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

    internal class RationalSimplificationRule : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryOperationHelper.GetFunctionName(BinaryOperator.Division);

        IImmutableList<string> IRule.ParameterNames => BinaryOperationHelper.ParameterNames;

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  We have 2 integer primitives
            if (left is PrimitiveExpression leftPe
                && leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                && right is PrimitiveExpression rightPe
                && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
            {
                var numerator = leftPe.ToInteger();
                var denominator = rightPe.ToInteger();

                if (denominator == 0)
                {   //  Division by zero
                    return null;
                }
                else if (denominator == 1)
                {   //  Division by 1
                    return PrimitiveExpression.Create(numerator);
                }
                else if (denominator == -1)
                {   //  Division by -1
                    return PrimitiveExpression.Create(-numerator);
                }
                else
                {   //  Try to simplify by greatest common denominator
                    var gcd = (int)BigInteger.GreatestCommonDivisor(numerator, denominator);

                    if (gcd == 1)
                    {
                        return null;
                    }
                    else
                    {
                        return new FunctionInvokeExpression(
                            NamespaceConstants.SYS,
                            BinaryOperator.Division.ToString().ToLower(),
                            ImmutableArray.Create<ExpressionBase>(
                                PrimitiveExpression.Create(numerator / gcd),
                                PrimitiveExpression.Create(denominator / gcd)));
                    }
                }
            }

            return null;
        }
    }
}
