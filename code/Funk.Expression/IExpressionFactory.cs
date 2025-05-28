using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal interface IExpressionFactory
    {
        ExpressionBase Create(ExpressionScript script);
    }
}