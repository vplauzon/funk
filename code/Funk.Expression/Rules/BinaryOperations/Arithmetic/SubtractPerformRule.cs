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
    internal class SubtractPerformRule : BinaryArithmeticPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.Substract;

        protected override ExpressionBase? Transform(int left, int right)
        {
            return PrimitiveExpression.Create(left - right);
        }

        protected override ExpressionBase? Transform(int left, double right)
        {
            return PrimitiveExpression.Create(left - right);
        }

        protected override ExpressionBase? Transform(double left, int right)
        {
            return PrimitiveExpression.Create(left - right);
        }

        protected override ExpressionBase? Transform(double left, double right)
        {
            return PrimitiveExpression.Create(left - right);
        }
    }
}
