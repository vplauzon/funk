using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Logical
{
    internal class EqualityPerformRule : BinaryLogicalPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.Equality;

        protected override ExpressionBase? Transform(bool left, bool right)
        {
            return PrimitiveExpression.Create(left == right);
        }
    }
}
