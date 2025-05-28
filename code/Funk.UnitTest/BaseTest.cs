using Funk.Expression;

namespace Funk.UnitTest
{
    public abstract class BaseTest
    {
        protected ExpressionBase ToExpression(string script)
        {
            var expression = ExpressionBuilder.Create(script);

            if (expression == null)
            {
                throw new NullReferenceException($"Script returns null expression:  '{script}'");
            }

            return expression;
        }

        protected bool ToBoolean(string script)
        {
            return bool.Parse(ToExpression(script).ToString());
        }

        protected int ToInteger(string script)
        {
            return int.Parse(ToExpression(script).ToString());
        }

        protected double ToFloat(string script)
        {
            return double.Parse(ToExpression(script).ToString());
        }

        protected string ToStringValue(string script)
        {
            var processedScript = ToExpression(script).ToString();

            if (processedScript.Length>=2
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