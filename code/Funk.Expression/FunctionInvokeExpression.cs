using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    public record FunctionInvokeExpression(
        string FunctionNamespace,
        string FunctionName,
        IImmutableList<FunctionParameter> Parameters) : ExpressionBase;
}