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
        public static ExpressionBase Create(string scriptText)
        {
            var script = ScriptParser.ParseScript(scriptText);

            if (script.Statements.Count() == 1)
            {
                var statement = script.Statements[0];

                if (statement.Expression != null)
                {
                    if (statement.Expression.Primitive != null)
                    {
                        if (statement.Expression.Primitive.Boolean != null)
                        {
                            return PrimitiveExpression.Create(
                                statement.Expression.Primitive.Boolean);
                        }
                        else if (statement.Expression.Primitive.Integer != null)
                        {
                            return PrimitiveExpression.Create(
                                statement.Expression.Primitive.Integer);
                        }
                        else if (statement.Expression.Primitive.Float != null)
                        {
                            return PrimitiveExpression.Create(
                                statement.Expression.Primitive.Float);
                        }
                        else if (statement.Expression.Primitive.String != null)
                        {
                            return PrimitiveExpression.Create(
                                statement.Expression.Primitive.String);
                        }
                        else
                        {
                            throw new InvalidDataException("An unknown primitive is defined");
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("A non-primitive expression is defined");
                    }
                }
                else
                {
                    throw new InvalidDataException("A rule is defined");
                }
            }
            else
            {
                throw new InvalidDataException($"Statement count is {script.Statements.Count()}");
            }
            throw new NotImplementedException();
        }
    }
}