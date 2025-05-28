using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    public static class ExpressionBuilder
    {
        public static ExpressionBase? Create(string scriptText)
        {
            var script = ScriptParser.ParseScript(scriptText);

            if (script.Rules.Any())
            {
                throw new NotSupportedException("Rules aren't supported");
            }
            if (script.Expression != null)
            {
                if (script.Expression.Primitive != null)
                {
                    return PrimitiveExpression.Create(script.Expression);
                }
                else
                {
                    throw new NotSupportedException("Non-primitive expression");
                }
            }
            else
            {
                return null;
            }
        }
    }
}