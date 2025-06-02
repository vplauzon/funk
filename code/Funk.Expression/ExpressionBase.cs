using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    public abstract record ExpressionBase
    {
        internal abstract T Visit<T>(IExpressionVisitor<T> visitor);
    }
}