namespace Funk.Expression.Rules
{
    using Funk.Expression.Expressions;
    using Funk.Parsing;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class RationalAddRule : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryArithmeticHelper.GetFunctionName(BinaryArithmeticOperand.Add);

        IImmutableList<string> IRule.ParameterNames => BinaryArithmeticHelper.ParameterNames;

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  We have 2 divisions
            if (left is FunctionInvokeExpression leftFi
                && leftFi.Namespace == NamespaceConstants.SYS
                && right is FunctionInvokeExpression rightFi
                && rightFi.Namespace == NamespaceConstants.SYS)
            {
                var leftRational = GetRational(leftFi);
                var rightRational = GetRational(rightFi);

                //  Both left & right are rational
                if (leftRational != null && rightRational != null)
                {
                    var l = leftRational.Value;
                    var r = rightRational.Value;

                    if (l.Denominator == r.Denominator)
                    {   //  Simplest sum
                        return new FunctionInvokeExpression(
                            NamespaceConstants.SYS,
                            BinaryArithmeticHelper.GetFunctionName(BinaryArithmeticOperand.Division),
                            ImmutableArray.Create<ExpressionBase>(
                                PrimitiveExpression.Create(l.Numerator + r.Numerator),
                                PrimitiveExpression.Create(l.Denominator)));
                    }
                    else
                    {   //  a/b+c/d = (a*d+b*c)/(c*d)
                        var numerator =
                            l.Numerator * r.Denominator + l.Denominator * r.Numerator;
                        var denominator = l.Denominator * r.Denominator;

                        return new FunctionInvokeExpression(
                            NamespaceConstants.SYS,
                            BinaryArithmeticOperand.Division.ToString().ToLower(),
                            ImmutableArray.Create<ExpressionBase>(
                                PrimitiveExpression.Create(numerator),
                                PrimitiveExpression.Create(denominator)));
                    }
                }
            }

            return null;
        }

        private (int Numerator, int Denominator)? GetRational(FunctionInvokeExpression div)
        {
            var left = div.Parameters[0];
            var right = div.Parameters[1];

            //  We have 2 integer primitives
            if (left is PrimitiveExpression leftPe
                && leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                && right is PrimitiveExpression rightPe
                && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
            {
                var numerator = leftPe.ToInteger();
                var denominator = rightPe.ToInteger();

                return (numerator, denominator);
            }

            return null;
        }
    }
}
