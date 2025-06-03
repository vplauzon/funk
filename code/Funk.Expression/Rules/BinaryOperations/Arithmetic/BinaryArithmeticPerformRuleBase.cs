using Funk.Expression.Expressions;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Rules.BinaryOperations.Arithmetic
{
    internal abstract class BinaryArithmeticPerformRuleBase : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryOperationHelper.GetFunctionName(BinaryOperator);

        IImmutableList<string> IRule.ParameterNames => BinaryOperationHelper.ParameterNames;

        protected abstract BinaryOperator BinaryOperator { get; }

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  We have 2 primitives
            if (left is PrimitiveExpression leftPe
                && right is PrimitiveExpression rightPe)
            {
                //  We have two numbers
                if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    return Transform(leftPe.ToInteger(), rightPe.ToInteger());
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return Transform(leftPe.ToInteger(), rightPe.ToFloat());
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    return Transform(leftPe.ToFloat(), rightPe.ToInteger());
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return Transform(leftPe.ToFloat(), rightPe.ToFloat());
                }
            }

            return null;
        }

        protected abstract ExpressionBase? Transform(int left, int right);

        protected abstract ExpressionBase? Transform(int left, double right);

        protected abstract ExpressionBase? Transform(double left, int right);

        protected abstract ExpressionBase? Transform(double left, double right);
    }
}