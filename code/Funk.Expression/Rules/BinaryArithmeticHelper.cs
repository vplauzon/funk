using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression.Rules
{
    internal static class BinaryArithmeticHelper
    {
        #region Constructors
        static BinaryArithmeticHelper()
        {
            ParameterNames = ImmutableArray.Create("a", "b");
        }
        #endregion

        public static IImmutableList<string> ParameterNames { get; }

        public static string GetFunctionName(BinaryArithmeticOperand operand)
        {
            return operand.ToString().ToLower();
        }
    }
}