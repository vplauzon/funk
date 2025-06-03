using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Rules
{
    internal static class BinaryOperationHelper
    {
        #region Constructors
        static BinaryOperationHelper()
        {
            ParameterNames = ImmutableArray.Create("a", "b");
        }
        #endregion

        public static IImmutableList<string> ParameterNames { get; }

        public static string GetFunctionName(BinaryOperator operand)
        {
            return operand.ToString().ToLower();
        }
    }
}