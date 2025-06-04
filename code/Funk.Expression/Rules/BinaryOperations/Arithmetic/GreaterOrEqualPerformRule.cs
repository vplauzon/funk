using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Arithmetic
{
    internal class GreaterOrEqualPerformRule : BinaryArithmeticPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.GreaterOrEqual;

        protected override ExpressionBase? Transform(int left, int right)
        {
            return PrimitiveExpression.Create(left >= right);
        }

        protected override ExpressionBase? Transform(int left, double right)
        {
            return PrimitiveExpression.Create(left >= right);
        }

        protected override ExpressionBase? Transform(double left, int right)
        {
            return PrimitiveExpression.Create(left >= right);
        }

        protected override ExpressionBase? Transform(double left, double right)
        {
            return PrimitiveExpression.Create(left >= right);
        }
    }
}
