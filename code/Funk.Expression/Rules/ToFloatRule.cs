﻿using Funk.Expression.Expressions;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Rules
{
    internal class ToFloatRule : IRule
    {
        string IRule.Namespace => "sys";

        string IRule.Name => "toFloat";

        ExpressionBase? IRule.Transform(IImmutableList<FunctionParameter> parameters)
        {   //  We have 1 parameter
            if (parameters.Count() == 1)
            {
                var expression = parameters[0].Expression;

                //  The expression is a primitive
                if (expression is PrimitiveExpression primitiveExpression)
                {
                    if (primitiveExpression.PrimitiveCategory == PrimitiveCategory.Integer)
                    {
                        return PrimitiveExpression.Create(
                            (double)primitiveExpression.ToInteger());
                    }
                    else if (primitiveExpression.PrimitiveCategory == PrimitiveCategory.Float)
                    {
                        return primitiveExpression;
                    }
                }
                //  Expression is a function invoke
                else if (expression is FunctionInvokeExpression functionInvoke)
                {   //  Expression is an integer division (a rational number)
                    if (functionInvoke.Namespace == "sys"
                        && functionInvoke.Name == BinaryArithmeticOperand.Division.ToString().ToLower()
                        && functionInvoke.Parameters.Count == 2
                        && functionInvoke.Parameters[0].Expression is PrimitiveExpression leftExpression
                        && leftExpression.PrimitiveCategory == PrimitiveCategory.Integer
                        && functionInvoke.Parameters[1].Expression is PrimitiveExpression rightExpression
                        && rightExpression.PrimitiveCategory == PrimitiveCategory.Integer)
                    {
                        return PrimitiveExpression.Create(
                            (double)leftExpression.ToInteger()/rightExpression.ToInteger());
                    }
                }
            }

            return null;
        }
    }
}