using Funk.Expression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal interface IExpressionVisitor<T>
    {
        T VisitPrimitive(PrimitiveExpression expression);

        T VisitFunctionInvoke(FunctionInvokeExpression expression);

        T VisitParameterAccess(ParameterAccessExpression expression);
    }
}