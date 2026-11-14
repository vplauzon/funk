using System;
using System.Collections.Generic;
using System.Text;

namespace Funk.Runtime.Core
{
    public class BoundFunctionEvaluation : FunctionEvaluationBase
    {
        internal BoundFunctionEvaluation(FunctionDefinitionPointer functionDefinition)
        {
            FunctionDefinition = functionDefinition;
        }

        public FunctionDefinitionPointer FunctionDefinition { get; }

        public override T Visit<T>(
            Func<PrimitiveFunctionEvaluation, T> primitiveVisitor,
            Func<NodeFunctionEvaluation, T> nodeVisitor,
            Func<UnboundFunctionEvaluation, T> unboundVisitor,
            Func<BoundFunctionEvaluation, T> boundVisitor)
        {
            return boundVisitor(this);
        }

        public override string ToScriptText()
        {
            return $"<Bound> {FunctionDefinition.FullName}()";
        }
    }
}