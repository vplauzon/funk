using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public abstract class FunctionEvaluationBase
    {
        public abstract T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor);
    }
}