using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Logical
{
    internal class AndPerformRule : BinaryLogicalPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.And;

        protected override ExpressionBase? Transform(bool left, bool right)
        {
            return PrimitiveExpression.Create(left && right);
        }
    }
}
