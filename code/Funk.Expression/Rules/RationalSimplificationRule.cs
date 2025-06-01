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
        private static readonly IImmutableList<string> _parameterNames =
            ImmutableArray.Create("a", "b");

        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryArithmeticOperand.Division.ToString().ToLower();

        IImmutableList<string> IRule.ExpectedParameterNames => _parameterNames;

        ExpressionBase? IRule.Transform(IImmutableList<FunctionParameter> parameters)
        {   //  We have 2 parameters
            if (parameters.Count() == 2)
            {
                var left = parameters[0].Expression;
                var right = parameters[1].Expression;

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
                                BinaryArithmeticOperand.Division.ToString().ToLower(),
                                ImmutableArray<FunctionParameter>.Empty
                                .Add(new FunctionParameter(
                                    null,
                                    PrimitiveExpression.Create(numerator / gcd)))
                                .Add(new FunctionParameter(
                                    null,
                                    PrimitiveExpression.Create(denominator / gcd))));
                        }
                    }
                }
            }

            return null;
        }
    }
}