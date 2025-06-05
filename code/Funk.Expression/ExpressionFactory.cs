using Funk.Expression.Expressions;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal static class ExpressionFactory
    {
        public static ExpressionBase Create(ExpressionScript script)
        {
            var orderedScript = OrderArithmeticExpressions(script) ?? script;

            return CreateFromOrderedScript(orderedScript);
        }

        private static ExpressionBase CreateFromOrderedScript(ExpressionScript script)
        {
            if (script.Primitive != null)
            {
                return PrimitiveExpression.Create(script.Primitive);
            }
            else if (script.BinaryOperation != null)
            {
                return FunctionInvokeExpression.Create(script.BinaryOperation);
            }
            else if (script.FunctionInvoke != null)
            {
                return FunctionInvokeExpression.Create(script.FunctionInvoke);
            }
            else if (script.Parenthesis != null)
            {
                return CreateFromOrderedScript(script.Parenthesis);
            }
            else if (script.ParameterAccess != null)
            {
                return ParameterAccessExpression.Create(script.ParameterAccess);
            }
            else if (script.If != null)
            {
                return IfExpression.Create(script.If);
            }
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
        }

        #region Order arithmetic
        private static ExpressionScript? OrderArithmeticExpressions(ExpressionScript script)
        {
            if (script.Primitive != null)
            {
                return null;
            }
            else if (script.BinaryOperation != null)
            {
                var orderedArithmetic = OrderArithmetic(script);

                return orderedArithmetic;
            }
            else if (script.FunctionInvoke != null)
            {
                return OrderArithmeticFunctionInvoke(script);
            }
            else if (script.Parenthesis != null)
            {
                var orderedScript = OrderArithmeticExpressions(script.Parenthesis);

                return orderedScript == null
                    ? null
                    : new ExpressionScript(Parenthesis: orderedScript);
            }
            else if (script.ParameterAccess != null)
            {
                return OrderArithmeticParameterAccess(script);
            }
            else if (script.If != null)
            {
                return OrderArithmeticIf(script);
            }
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
        }

        private static ExpressionScript? OrderArithmeticFunctionInvoke(ExpressionScript script)
        {
            var orderedParameters = script.FunctionInvoke!.Parameters
                .Select(p => OrderArithmeticExpressions(p))
                .ToImmutableArray();

            if (orderedParameters.Any(e => e != null))
            {
                var reconstructedParameters = script.FunctionInvoke.Parameters
                    .Zip(orderedParameters, (orig, ordered) => ordered ?? orig)
                    .ToArray();
                var newFunctionInvoke = new FunctionInvokeScript(
                        script.FunctionInvoke.Namespace,
                        script.FunctionInvoke.Name,
                        reconstructedParameters);

                return new ExpressionScript(FunctionInvoke: newFunctionInvoke);
            }
            else
            {
                return null;
            }
        }

        private static ExpressionScript? OrderArithmeticParameterAccess(ExpressionScript script)
        {
            var orderedExpressionScript =
                OrderArithmeticExpressions(script.ParameterAccess!.Expression);

            return orderedExpressionScript == null
                ? null
                : new ExpressionScript(
                    ParameterAccess: new ParameterAccessScript(
                        orderedExpressionScript,
                        script.ParameterAccess.Names));
        }

        private static ExpressionScript? OrderArithmeticIf(ExpressionScript script)
        {
            var ifValue = script.If!;
            var ifThens = ifValue.IfThens
                .Select(e => new
                {
                    Condition = OrderArithmetic(e.Condition),
                    ThenExpression = OrderArithmetic(e.ThenExpression)
                })
                .ToImmutableArray();
            var elseExpression = OrderArithmeticExpressions(ifValue.ElseExpression);
            var reconstructIfThens = ifThens
                .Zip(ifValue.IfThens, (ordered, orig) => new IfThenScript(
                    ordered.Condition ?? orig.Condition,
                    ordered.ThenExpression ?? orig.ThenExpression));

            return ifThens.All(i => i.Condition == null && i.ThenExpression == null)
                && elseExpression == null
                ? null
                : new ExpressionScript(
                    If: new IfScript(
                        reconstructIfThens.ToImmutableArray(),
                        elseExpression ?? ifValue.ElseExpression));
        }

        private static ExpressionScript? OrderArithmetic(ExpressionScript arithmeticBinary)
        {   //  Shunting Yard algorithm
            //  Local function for precedence
            int GetOperatorPrecedence(BinaryOperator operand) => operand switch
            {
                BinaryOperator.Power => 4,
                BinaryOperator.Division => 3,
                BinaryOperator.Product => 2,
                BinaryOperator.Add or BinaryOperator.Substract => 1,
                _ => throw new NotSupportedException($"Unknown operand: {operand}")
            };

            // Convert expression tree to flat list
            var expressions = new Stack<ExpressionScript>();
            var operators = new Stack<BinaryOperator>();

            void ProcessExpression(ExpressionScript expression)
            {
                if (expression.BinaryOperation != null)
                {
                    var binary = expression.BinaryOperation;

                    ProcessExpression(binary.Left);
                    while (operators.Count > 0
                        && GetOperatorPrecedence(operators.Peek()) >= GetOperatorPrecedence(binary.Operator))
                    {
                        // Pop and create new binary expression
                        var right = expressions.Pop();
                        var left = expressions.Pop();
                        var op = operators.Pop();

                        expressions.Push(new ExpressionScript(
                            BinaryOperation: new BinaryOperationScript(op, left, right)));
                    }
                    operators.Push(binary.Operator);
                    ProcessExpression(binary.Right);
                }
                else
                {
                    expressions.Push(expression);
                }
            }

            // Process the initial expression
            ProcessExpression(arithmeticBinary);

            if (operators.Count > 1)
            {
                // Process remaining operators
                while (operators.Count > 0)
                {
                    var right = expressions.Pop();
                    var left = expressions.Pop();
                    var op = operators.Pop();

                    expressions.Push(
                        new ExpressionScript(
                            BinaryOperation: new BinaryOperationScript(op, left, right)));
                }

                return expressions.Pop();
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
