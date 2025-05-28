using Funk.Expression;
using Funk.Parsing;

namespace Funk.UnitTest
{
    public abstract class BaseTest
    {
        private readonly ExpressionBuilder _expressionBuilder = new();

        protected ExpressionBase ToExpression(string scriptText)
        {
            var script = ScriptParser.ParseScript(scriptText);
            var expression = _expressionBuilder.ProcessScript(script);

            if (expression == null)
            {
                throw new NullReferenceException($"Script returns null expression:  '{script}'");
            }

            return expression;
        }

        protected ExpressionBase ToTransformedExpression(string scriptText)
        {
            var expression = ToExpression(scriptText);
            var transformedExpression = _expressionBuilder.Transform(expression);

            return transformedExpression;
        }

        protected bool ToBoolean(string script)
        {
            return bool.Parse(ToTransformedExpression(script).ToString());
        }

        protected int ToInteger(string script)
        {
            return int.Parse(ToTransformedExpression(script).ToString());
        }

        protected double ToFloat(string script)
        {
            return double.Parse(ToTransformedExpression(script).ToString());
        }

        protected string ToStringValue(string script)
        {
            var processedScript = ToTransformedExpression(script).ToString();

            if (processedScript.Length >= 2
                &&
                ((processedScript.First() == '\'' && processedScript.Last() == '\'')
                || (processedScript.First() == '"' && processedScript.Last() == '"')))
            {
                return processedScript.Substring(1, processedScript.Length - 2);
            }

            throw new InvalidCastException($"Expression isn't a string:  '{processedScript}'");
        }
    }
}