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
    internal class DivisionPerformRule : BinaryArithmeticPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.Division;

        protected override ExpressionBase? Transform(int left, int right)
        {   //  This is a rational number and is dealt with rational number rules
            return null;
        }

        protected override ExpressionBase? Transform(int left, double right)
        {
            return PrimitiveExpression.Create(left / right);
        }

        protected override ExpressionBase? Transform(double left, int right)
        {
            return PrimitiveExpression.Create(left / right);
        }

        protected override ExpressionBase? Transform(double left, double right)
        {
            return PrimitiveExpression.Create(left / right);
        }
    }
}
