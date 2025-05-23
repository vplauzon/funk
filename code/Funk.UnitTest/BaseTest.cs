using Funk.Expression;

namespace Funk.UnitTest
{
    public abstract class BaseTest
    {
        protected PrimitiveExpression ToPrimitive(string script)
        {
            var expression = ExpressionBuilder.Create(script);

            if (expression is PrimitiveExpression primitiveExpression)
            {
                return primitiveExpression;
            }
            else
            {
                throw new InvalidCastException("Expression isn't a primitive");
            }
        }
    }
}