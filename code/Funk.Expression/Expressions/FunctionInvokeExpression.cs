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
        IImmutableList<FunctionParameter> Parameters) : ExpressionBase
    {
        #region Constructors
        public static FunctionInvokeExpression Create(BinaryArithmeticScript script)
        {
            return new FunctionInvokeExpression(
                NamespaceConstants.SYS,
                script.Operand.ToString().ToLower(),
                ImmutableArray<FunctionParameter>.Empty
                .Add(new FunctionParameter(null, ExpressionFactory.Create(script.Left)))
                .Add(new FunctionParameter(null, ExpressionFactory.Create(script.Right))));
        }

        public static FunctionInvokeExpression Create(FunctionInvokeScript functionInvoke)
        {
            var parameters = functionInvoke.Parameters
                .Select(p => new FunctionParameter(
                    p.Name,
                    ExpressionFactory.Create(p.Expression)))
                .ToImmutableArray();

            return new FunctionInvokeExpression(
                functionInvoke.Namespace ?? NamespaceConstants.SYS,
                functionInvoke.Name,
                parameters);
        }
        #endregion

        public override string ToString()
        {
            var parameters = Parameters
                .Select(p => p.Name == null
                ? $"({p.Expression.ToString()})"
                : $"({p.Name}={p.Expression.ToString()})");
            var parametersText = string.Join(", ", parameters);

            return $"{Namespace}.{Name}({parametersText})";
        }
    }
}