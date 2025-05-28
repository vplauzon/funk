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
                    if (script.Expression.Primitive.Boolean != null)
                    {
                        return PrimitiveExpression.Create(
                            script.Expression.Primitive.Boolean);
                    }
                    else if (script.Expression.Primitive.Integer != null)
                    {
                        return PrimitiveExpression.Create(
                            script.Expression.Primitive.Integer);
                    }
                    else if (script.Expression.Primitive.Float != null)
                    {
                        return PrimitiveExpression.Create(
                            script.Expression.Primitive.Float);
                    }
                    else if (script.Expression.Primitive.String != null)
                    {
                        return PrimitiveExpression.Create(
                            script.Expression.Primitive.String);
                    }
                    else
                    {
                        throw new InvalidDataException("An unknown primitive is defined");
                    }
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