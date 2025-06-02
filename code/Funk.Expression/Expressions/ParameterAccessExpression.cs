using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Expressions
{
    internal record ParameterAccessExpression(
        string Name,
        ExpressionBase Expression) : ExpressionBase
    {
        #region Constructors
        public static ParameterAccessExpression Create(ParameterAccessScript parameterAccessScript)
        {
            return new ParameterAccessExpression(
                parameterAccessScript.Name,
                ExpressionFactory.Create(parameterAccessScript.Expression));
        }
        #endregion

        internal override T Visit<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitParameterAccess(this);
        }

        public override string ToString()
        {
            return $"{Expression}.{Name}";
        }
    }
}