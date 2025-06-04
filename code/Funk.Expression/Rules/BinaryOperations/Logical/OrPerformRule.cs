using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Logical
{
    internal class OrPerformRule : BinaryLogicalPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.Or;

        protected override ExpressionBase? Transform(bool left, bool right)
        {
            return PrimitiveExpression.Create(left || right);
        }
    }
}
