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
    internal record IfThenExpression(ExpressionBase Condition, ExpressionBase ThenExpression);
}