using Funk.Expression.Expressions;
using Funk.Parsing;
using System.Collections.Immutable;

namespace Funk.Expression.Rules.BinaryOperations.Logical
{
    internal abstract class BinaryLogicalPerformRuleBase : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryOperationHelper.GetFunctionName(BinaryOperator);

        IImmutableList<string> IRule.ParameterNames => BinaryOperationHelper.ParameterNames;

        protected abstract BinaryOperator BinaryOperator { get; }

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  Two primitives
            if (left is PrimitiveExpression leftPe
                && right is PrimitiveExpression rightPe
                //  Two booleans
                && leftPe.PrimitiveCategory == PrimitiveCategory.Boolean
                && rightPe.PrimitiveCategory == PrimitiveCategory.Boolean)
            {
                return Transform(leftPe.ToBoolean(), rightPe.ToBoolean());
            }

            return null;
        }

        protected abstract ExpressionBase? Transform(bool left, bool right);
    }
}
