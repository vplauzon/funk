using Funk.Expression.Rules;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Expressions
{
    internal record FunctionInvokeExpression(
        string Namespace,
        string Name,
        IImmutableList<ExpressionBase> Parameters) : ExpressionBase
    {
        #region Constructors
        public static FunctionInvokeExpression Create(BinaryArithmeticScript script)
        {
            return new FunctionInvokeExpression(
                NamespaceConstants.SYS,
                BinaryArithmeticHelper.GetFunctionName(script.Operand),
                ImmutableArray<ExpressionBase>.Empty
                .Add(ExpressionFactory.Create(script.Left))
                .Add(ExpressionFactory.Create(script.Right)));
        }

        public static FunctionInvokeExpression Create(FunctionInvokeScript functionInvoke)
        {
            var parameters = functionInvoke.Parameters
                .Select(p => ExpressionFactory.Create(p))
                .ToImmutableArray();

            return new FunctionInvokeExpression(
                functionInvoke.Namespace ?? NamespaceConstants.SYS,
                functionInvoke.Name,
                parameters);
        }
        #endregion

        internal override T Visit<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitFunctionInvoke(this);
        }

        public override string ToString()
        {
            var parameters = Parameters
                .Select(p => p.ToString());
            var parametersText = string.Join(", ", parameters);

            return $"{Namespace}.{Name}({parametersText})";
        }
    }
}