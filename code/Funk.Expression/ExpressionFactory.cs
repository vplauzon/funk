using Funk.Expression.Expressions;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal static class ExpressionFactory
    {
        public static ExpressionBase Create(ExpressionScript script)
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
                return Create(script.Parenthesis);
            }
            else
            {
                throw new NotSupportedException("Unknown expression script");
            }
        }
    }
}