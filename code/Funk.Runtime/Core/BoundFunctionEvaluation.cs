using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class BoundFunctionEvaluation : FunctionEvaluationBase
    {
        internal BoundFunctionEvaluation(IFunctionLibrary functionLibrary, string functionName)
        {
            FunctionLibrary = functionLibrary;
            FunctionName = functionName;
        }

        internal IFunctionLibrary FunctionLibrary { get; }

        public string FunctionName { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor,
            Func<BoundFunctionEvaluation, T> boundVisitor)
        {
            return boundVisitor(this);
        }
    }
}