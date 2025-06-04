using Funk.Expression.Expressions;
using Funk.Parsing;
using System.Collections.Immutable;

namespace Funk.Expression.Rules.BinaryOperations.Inequalities
{
    internal abstract class BinaryInequalityPerformRuleBase : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => BinaryOperationHelper.GetFunctionName(BinaryOperator);

        IImmutableList<string> IRule.ParameterNames => BinaryOperationHelper.ParameterNames;

        protected abstract BinaryOperator BinaryOperator { get; }

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            if (left is PrimitiveExpression leftPe
                && right is PrimitiveExpression rightPe)
            {
                // Both integers
                if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    return Transform(leftPe.ToInteger(), rightPe.ToInteger());
                }
                // Both floats
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return Transform(leftPe.ToFloat(), rightPe.ToFloat());
                }
                // Integer and float
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return Transform(leftPe.ToInteger(), rightPe.ToFloat());
                }
                // Float and integer
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    return Transform(leftPe.ToFloat(), rightPe.ToInteger());
                }
            }

            return null;
        }

        protected abstract ExpressionBase? Transform(int left, int right);
        protected abstract ExpressionBase? Transform(double left, double right);
        protected abstract ExpressionBase? Transform(int left, double right);
        protected abstract ExpressionBase? Transform(double left, int right);
    }
}
