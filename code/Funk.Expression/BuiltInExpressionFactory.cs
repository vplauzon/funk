using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal class BuiltInExpressionFactory : IExpressionFactory
    {
        ExpressionBase IExpressionFactory.Create(ExpressionScript script)
        {

            if (script.Primitive != null)
            {
                return PrimitiveExpression.Create(script);
            }
            else if (script.ArithmeticBinary != null)
            {
                return FunctionInvokeExpression.Create(script.ArithmeticBinary, this);
            }
            else
            {
                throw new NotSupportedException("Unkown expression script");
            }
        }
    }
}