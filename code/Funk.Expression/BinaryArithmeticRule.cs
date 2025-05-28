using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal class BinaryArithmeticRule : IRule
    {
        string IRule.Namespace => "sys";

        string IRule.Name => throw new NotImplementedException();

        ExpressionBase? IRule.Transform(ExpressionBase expression)
        {
            throw new NotImplementedException();
        }
    }
}