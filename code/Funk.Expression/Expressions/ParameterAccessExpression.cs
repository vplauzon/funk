using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Funk.Expression.Expressions
{
    internal record ParameterAccessExpression(
        string Name,
        ExpressionBase Expression) : ExpressionBase
    {
        #region Constructors
        public static ParameterAccessExpression Create(
            ParameterAccessScript parameterAccessScript)
        {
            if (!parameterAccessScript.Names.Any())
            {
                throw new InvalidDataException(
                    $"No names provided to access {parameterAccessScript.Expression}");
            }

            var expression = new ParameterAccessExpression(
                parameterAccessScript.Names.First(),
                ExpressionFactory.Create(parameterAccessScript.Expression));

            //  Wrap recursively
            foreach (var name in parameterAccessScript.Names.Skip(1))
            {
                expression = new ParameterAccessExpression(name, expression);
            }

            return expression;
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