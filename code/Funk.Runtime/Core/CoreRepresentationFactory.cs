using Funk.Parsing;

namespace Funk.Runtime.Core
{
    internal static class CoreRepresentationFactory
    {
        public static FunctionEvaluationBase Create(FunctionEvaluationNode node)
        {
            if (node.Primitive != null)
            {
                if (node.Primitive.Integer != null)
                {
                    return new PrimitiveFunctionEvaluation(node.Primitive.Integer.Value);
                }
                if (node.Primitive.Real != null)
                {
                    return new PrimitiveFunctionEvaluation(node.Primitive.Real.Value);
                }
                if (node.Primitive.String != null)
                {
                    return new PrimitiveFunctionEvaluation(node.Primitive.String);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}