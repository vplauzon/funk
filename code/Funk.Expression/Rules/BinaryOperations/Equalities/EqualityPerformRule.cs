using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Equalities
{
    internal class EqualityPerformRule : BinaryEqualityPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.Equality;

        protected override ExpressionBase? Transform(string left, string right)
        {
            return PrimitiveExpression.Create(left == right);
        }

        protected override ExpressionBase? Transform(bool left, bool right)
        {
            return PrimitiveExpression.Create(left == right);
        }

        protected override ExpressionBase? Transform(int left, int right)
        {
            return PrimitiveExpression.Create(left == right);
        }

        protected override ExpressionBase? Transform(double left, double right)
        {
            return PrimitiveExpression.Create(left == right);
        }

        protected override ExpressionBase? Transform(int left, double right)
        {
            return PrimitiveExpression.Create(left == right);
        }

        protected override ExpressionBase? Transform(double left, int right)
        {
            return PrimitiveExpression.Create(left == right);
        }
    }
}
