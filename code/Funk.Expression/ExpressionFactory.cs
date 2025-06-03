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
            var orderedScript = OrderArithmetricExpressions(script) ?? script;

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
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
        }

        private static ExpressionScript? OrderArithmetricExpressions(ExpressionScript script)
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
                var orderedParameters = script.FunctionInvoke.Parameters
                    .Select(p => OrderArithmetricExpressions(p))
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
            else if (script.Parenthesis != null)
            {
                var orderedScript = OrderArithmetricExpressions(script.Parenthesis);

                return orderedScript == null
                    ? null
                    : new ExpressionScript(Parenthesis: orderedScript);
            }
            else if (script.ParameterAccess != null)
            {
                var orderedExpressionScript =
                    OrderArithmetricExpressions(script.ParameterAccess.Expression);

                return orderedExpressionScript == null
                    ? null
                    : new ExpressionScript(
                        ParameterAccess: new ParameterAccessScript(
                            orderedExpressionScript,
                            script.ParameterAccess.Names));
            }
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
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
    }
}
