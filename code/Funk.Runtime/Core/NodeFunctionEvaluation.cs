using Funk.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class NodeFunctionEvaluation : FunctionEvaluationBase
    {
        public NodeFunctionEvaluation(FunctionEvaluationNode functionEvaluationNode)
        {
            FunctionEvaluationNode = functionEvaluationNode;
        }

        public FunctionEvaluationNode FunctionEvaluationNode { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor)
        {
            return nodeVisitor(this);
        }
    }
}