using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class PrimitiveFunctionEvaluation : FunctionEvaluationBase
    {
        public PrimitiveFunctionEvaluation(bool value)
        {
            Value = value;
        }

        public PrimitiveFunctionEvaluation(long value)
        {
            Value = value;
        }

        public PrimitiveFunctionEvaluation(double value)
        {
            Value = value;
        }

        public PrimitiveFunctionEvaluation(string value)
        {
            Value = value;
        }

        public object Value { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor)
        {
            return primitiveVisitor(this);
        }
    }
}