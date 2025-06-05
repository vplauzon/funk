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
                if (script.If.TernaryIf != null)
                {
                    return IfExpression.Create(script.If.TernaryIf);
                }
                else if (script.If.ChainedIfElse != null)
                {
                    return IfExpression.Create(script.If.ChainedIfElse);
                }
                else
                {
                    throw new NotSupportedException("Unknown if expression script");
                }
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
            if (script.If!.TernaryIf != null)
            {
                var ifValue = script.If.TernaryIf;
                var condition = OrderArithmeticExpressions(ifValue.Condition);
                var trueExpression = OrderArithmeticExpressions(ifValue.TrueExpression);
                var falseExpression = OrderArithmeticExpressions(ifValue.FalseExpression);

                return condition == null && trueExpression == null && falseExpression == null
                    ? null
                    : new ExpressionScript(
                        If: new IfScript(TernaryIf: new TernaryIfScript(
                            condition ?? ifValue.Condition,
                            trueExpression ?? ifValue.TrueExpression,
                            falseExpression ?? ifValue.FalseExpression)));
            }
            else if (script.If.ChainedIfElse != null)
            {
                var ifValue = script.If.ChainedIfElse;
                var condition = OrderArithmeticExpressions(ifValue.Condition);
                var thenExpression = OrderArithmeticExpressions(ifValue.ThenExpression);
                var elseExpression = ifValue.ElseExpression == null
                    ? null
                    : OrderArithmeticExpressions(ifValue.ElseExpression);
                var elseIf = ifValue.ElseIfs
                    .Select(e => new
                    {
                        Expression = OrderArithmeticExpressions(e.ThenExpression),
                        Condition = OrderArithmeticExpressions(e.Condition)
                    })
                    .ToImmutableArray();
                var reconstructElseIf = elseIf
                    .Zip(ifValue.ElseIfs, (ordered, orig) => new
                    {
                        Expression = ordered.Expression ?? orig.ThenExpression,
                        Condition = ordered.Condition ?? orig.Condition,
                    })
                    .Select(o => new ElseIfClauseScript(o.Condition, o.Expression));

                return condition == null
                    && thenExpression == null
                    && elseExpression == null
                    && !elseIf.Any(o => o != null)
                    ? null
                    : new ExpressionScript(
                        If: new IfScript(ChainedIfElse: new ChainedIfElseScript(
                            condition ?? ifValue.Condition,
                            thenExpression ?? ifValue.ThenExpression,
                            elseIf.Any(o => o != null)
                            ? reconstructElseIf.ToImmutableArray()
                            : ifValue.ElseIfs,
                            elseExpression ?? ifValue.ElseExpression)));
            }
            else
            {
                throw new NotSupportedException("Unknown if script");
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
        #endregion
    }
}
