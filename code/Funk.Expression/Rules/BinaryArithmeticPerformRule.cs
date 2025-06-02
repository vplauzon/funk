namespace Funk.Expression.Rules
{
    using Funk.Expression.Expressions;
    using Funk.Parsing;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    internal class BinaryArithmeticPerformRule : IRule
    {
        private readonly BinaryArithmeticOperand _binaryArithmeticOperand;

        public BinaryArithmeticPerformRule(BinaryArithmeticOperand binaryArithmeticOperand)
        {
            _binaryArithmeticOperand = binaryArithmeticOperand;
        }

        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => _binaryArithmeticOperand.ToString().ToLower();

        IImmutableList<string> IRule.ParameterNames => BinaryArithmeticHelper.ParameterNames;

        ExpressionBase? IRule.Transform(IImmutableList<ExpressionBase> parameters)
        {
            var left = parameters[0];
            var right = parameters[1];

            //  We have 2 primitives
            if (left is PrimitiveExpression leftPe
                && right is PrimitiveExpression rightPe)
            {
                //  We have two numbers
                if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    if (_binaryArithmeticOperand != BinaryArithmeticOperand.Division)
                    {
                        return PrimitiveExpression.Create(
                            PerformOperand(leftPe.ToInteger(), rightPe.ToInteger()));
                    }
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return PrimitiveExpression.Create(
                        PerformOperand(leftPe.ToInteger(), rightPe.ToFloat()));
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                {
                    return PrimitiveExpression.Create(
                        PerformOperand(leftPe.ToFloat(), rightPe.ToInteger()));
                }
                else if (leftPe.PrimitiveCategory == PrimitiveCategory.Float
                    && rightPe.PrimitiveCategory == PrimitiveCategory.Float)
                {
                    return PrimitiveExpression.Create(
                        PerformOperand(leftPe.ToFloat(), rightPe.ToFloat()));
                }
            }

            return null;
        }

        #region Perform operand
        private object PerformOperand(int left, int right)
        {
            return _binaryArithmeticOperand switch
            {
                BinaryArithmeticOperand.Add => left + right,
                BinaryArithmeticOperand.Substract => left - right,
                BinaryArithmeticOperand.Product => left * right,
                BinaryArithmeticOperand.Division => left / right,
                BinaryArithmeticOperand.Power => Math.Pow(left, right),
                _ => throw new NotSupportedException(
                    $"Unsupported primitive type:  '{_binaryArithmeticOperand}'")
            };
        }

        private object PerformOperand(int left, double right)
        {
            return _binaryArithmeticOperand switch
            {
                BinaryArithmeticOperand.Add => left + right,
                BinaryArithmeticOperand.Substract => left - right,
                BinaryArithmeticOperand.Product => left * right,
                BinaryArithmeticOperand.Division => left / right,
                BinaryArithmeticOperand.Power => Math.Pow(left, right),
                _ => throw new NotSupportedException(
                    $"Unsupported primitive type:  '{_binaryArithmeticOperand}'")
            };
        }

        private object PerformOperand(double left, int right)
        {
            return _binaryArithmeticOperand switch
            {
                BinaryArithmeticOperand.Add => left + right,
                BinaryArithmeticOperand.Substract => left - right,
                BinaryArithmeticOperand.Product => left * right,
                BinaryArithmeticOperand.Division => left / right,
                BinaryArithmeticOperand.Power => Math.Pow(left, right),
                _ => throw new NotSupportedException(
                    $"Unsupported primitive type:  '{_binaryArithmeticOperand}'")
            };
        }

        private object PerformOperand(double left, double right)
        {
            return _binaryArithmeticOperand switch
            {
                BinaryArithmeticOperand.Add => left + right,
                BinaryArithmeticOperand.Substract => left - right,
                BinaryArithmeticOperand.Product => left * right,
                BinaryArithmeticOperand.Division => left / right,
                BinaryArithmeticOperand.Power => Math.Pow(left, right),
                _ => throw new NotSupportedException(
                    $"Unsupported primitive type:  '{_binaryArithmeticOperand}'")
            };
        }
        #endregion
    }
}
