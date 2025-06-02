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
            else if (script.ArithmeticBinary != null)
            {
                return FunctionInvokeExpression.Create(script.ArithmeticBinary);
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
            else if (script.ArithmeticBinary != null)
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

                    return new ExpressionScript(null, null, newFunctionInvoke, null, null);
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
                    : new ExpressionScript(null, null, null, orderedScript, null);
            }
            else if (script.ParameterAccess != null)
            {
                var orderedExpressionScript =
                    OrderArithmetricExpressions(script.ParameterAccess.Expression);

                return orderedExpressionScript == null
                    ? null
                    : new ExpressionScript(
                        null,
                        null,
                        null,
                        null,
                        new ParameterAccessScript(
                            script.ParameterAccess.Name,
                            orderedExpressionScript));
            }
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
        }

        private static ExpressionScript? OrderArithmetic(ExpressionScript arithmeticBinary)
        {   //  Shunting Yard algorithm
            // Local function for precedence
            int GetOperatorPrecedence(BinaryArithmeticOperand operand) => operand switch
            {
                BinaryArithmeticOperand.Power => 3,
                BinaryArithmeticOperand.Product or BinaryArithmeticOperand.Division => 2,
                BinaryArithmeticOperand.Add or BinaryArithmeticOperand.Substract => 1,
                _ => throw new NotSupportedException($"Unknown operand: {operand}")
            };

            // Convert expression tree to flat list
            var expressions = new Stack<ExpressionScript>();
            var operators = new Stack<BinaryArithmeticOperand>();

            void ProcessExpression(ExpressionScript expression)
            {
                if (expression.ArithmeticBinary != null)
                {
                    var binary = expression.ArithmeticBinary;

                    ProcessExpression(binary.Left);
                    while (operators.Count > 0
                        && GetOperatorPrecedence(operators.Peek()) >= GetOperatorPrecedence(binary.Operand))
                    {
                        // Pop and create new binary expression
                        var right = expressions.Pop();
                        var left = expressions.Pop();
                        var op = operators.Pop();

                        expressions.Push(new ExpressionScript(
                            null,
                            new BinaryArithmeticScript(op, left, right),
                            null,
                            null,
                            null));
                    }
                    operators.Push(binary.Operand);
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
                            null,
                            new BinaryArithmeticScript(op, left, right),
                            null,
                            null,
                            null));
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
