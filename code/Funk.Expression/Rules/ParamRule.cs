using Funk.Expression.Expressions;
using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Rules
{
    internal class ParamRule : IRule
    {
        string IRule.Namespace => NamespaceConstants.SYS;

        string IRule.Name => "param";

        ExpressionBase? IRule.Transform(IImmutableList<FunctionParameter> parameters)
        {   //  We have 2 parameters
            if (parameters.Count() == 2)
            {
                var expression = parameters[0].Expression;
                var indexExpression = parameters[1].Expression;

                //  The expression is a function invoke while the second is an integer
                if (indexExpression is PrimitiveExpression indexInteger
                    && indexInteger.PrimitiveCategory == PrimitiveCategory.Integer
                    && expression is FunctionInvokeExpression functionInvoke)
                {
                    var index = indexInteger.ToInteger();

                    if (index >= 0 && index < functionInvoke.Parameters.Count)
                    {
                        return functionInvoke.Parameters[index].Expression;
                    }
                }
            }

            return null;
        }
    }
}