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
    internal record IfExpression(
        IImmutableList<IfThenExpression> IfThenExpressions,
        ExpressionBase ElseExpression) : ExpressionBase
    {
        #region Constructors
        public static IfExpression Create(IfScript script)
        {
            var thenExpressions = script.IfThens
                .Select(e => new IfThenExpression(
                    ExpressionFactory.Create(e.Condition),
                    ExpressionFactory.Create(e.ThenExpression)))
                .ToImmutableArray();
            var elseExpression = ExpressionFactory.Create(script.ElseExpression);

            return new IfExpression(thenExpressions, elseExpression);
        }
        #endregion

        internal override T Visit<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitIf(this);
        }

        public override string ToString()
        {
            var firstIfThen = IfThenExpressions.First();
            var leadIfText = @$"if({firstIfThen.Condition})
{{
    return {firstIfThen.ThenExpression};
}}
";
            var otherIfsText = IfThenExpressions.Skip(1)
                .Select(e => $@"else if({e.Condition})
{{
    return {e.ThenExpression};
}}
");
            var elseText = $@"else
{{
    return {ElseExpression};
}}";

            return $"{leadIfText}{string.Join("", otherIfsText)}{elseText}";
        }
    }
}