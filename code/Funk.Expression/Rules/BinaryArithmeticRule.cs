﻿namespace Funk.Expression.Rules
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

    internal class BinaryArithmeticRule : IRule
    {
        private readonly BinaryArithmeticOperand _binaryArithmeticOperand;

        public BinaryArithmeticRule(BinaryArithmeticOperand binaryArithmeticOperand)
        {
            _binaryArithmeticOperand = binaryArithmeticOperand;
        }

        string IRule.Namespace => "sys";

        string IRule.Name => _binaryArithmeticOperand.ToString().ToLower();

        ExpressionBase? IRule.Transform(IImmutableList<FunctionParameter> parameters)
        {   //  We have 2 parameters
            if (parameters.Count() == 2)
            {
                var left = parameters[0].Expression;
                var right = parameters[1].Expression;

                //  We have 2 primitives
                if (left is PrimitiveExpression leftPe
                    && right is PrimitiveExpression rightPe)
                {
                    //  We have two numbers
                    if (leftPe.PrimitiveCategory == PrimitiveCategory.Integer
                        && rightPe.PrimitiveCategory == PrimitiveCategory.Integer)
                    {
                        if (_binaryArithmeticOperand == BinaryArithmeticOperand.Division)
                        {
                            return SimplifyRational(leftPe.ToInteger(), rightPe.ToInteger());
                        }
                        else
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
            }

            return null;
        }

        private ExpressionBase? SimplifyRational(int numerator, int denominator)
        {
            if (denominator == 0)
            {   //  Division by zero
                return null;
            }
            else if (denominator == 1)
            {
                return PrimitiveExpression.Create(numerator);
            }
            else if (denominator == -1)
            {
                return PrimitiveExpression.Create(-numerator);
            }
            else
            {
                var gcd = (int)BigInteger.GreatestCommonDivisor(numerator, denominator);

                if (gcd == 1)
                {
                    return null;
                }
                else
                {
                    return new FunctionInvokeExpression(
                        "sys",
                        BinaryArithmeticOperand.Division.ToString().ToLower(),
                        ImmutableArray<FunctionParameter>.Empty
                        .Add(new FunctionParameter(
                            null,
                            PrimitiveExpression.Create(numerator / gcd)))
                        .Add(new FunctionParameter(
                            null,
                            PrimitiveExpression.Create(denominator / gcd))));
                }
            }
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