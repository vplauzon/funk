using Funk.Expression.Expressions;
using Funk.Parsing;

namespace Funk.Expression.Rules.BinaryOperations.Logical
{
    internal class NonEqualityPerformRule : BinaryLogicalPerformRuleBase
    {
        protected override BinaryOperator BinaryOperator => BinaryOperator.NonEquality;

        protected override ExpressionBase? Transform(bool left, bool right)
        {
            return PrimitiveExpression.Create(left != right);
        }
    }
}
