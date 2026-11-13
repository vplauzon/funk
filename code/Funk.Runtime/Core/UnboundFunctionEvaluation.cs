using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class UnboundFunctionEvaluation : FunctionEvaluationBase
    {
        public UnboundFunctionEvaluation(string functionName)
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor)
        {
            return unboundVisitor(this);
        }
    }
}