using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class FunctionEvaluationResolver
    {
        public FunctionEvaluationResolver(FunctionEvaluationNode functionEvaluationNode)
        {
            FunctionEvaluation = new NodeFunctionEvaluation(functionEvaluationNode);
        }

        private FunctionEvaluationResolver(FunctionEvaluationBase functionEvaluation)
        {
            FunctionEvaluation = functionEvaluation;
        }

        public FunctionEvaluationBase FunctionEvaluation { get; }

        public async Task<FunctionEvaluationResolver> SimplifyAsync()
        {
            //  Switch the function evaluation to core representation
            var coreEvaluation = FunctionEvaluation.Visit(
                p => p,
                n => CoreRepresentationFactory.Create(n.FunctionEvaluationNode),
                u => u);
            var hasChanged = true;

            while (hasChanged)
            {
                var functionResolution = ResolveFunction(coreEvaluation);
                var evaluationResolution = EvaluateFunction(functionResolution ?? coreEvaluation);

                coreEvaluation = evaluationResolution ?? functionResolution ?? coreEvaluation;
                hasChanged = functionResolution != null || evaluationResolution != null;
            }

            return new FunctionEvaluationResolver(coreEvaluation);
        }

        private FunctionEvaluationBase? ResolveFunction(FunctionEvaluationBase evaluation)
        {
            var resolvedEvaluation = evaluation.Visit(
                p => null,
                n => null,
                u => ResolveUnboundFunction(u));

            return resolvedEvaluation;
        }

        private FunctionEvaluationBase? ResolveUnboundFunction(
            UnboundFunctionEvaluation evaluation)
        {
            throw new NotImplementedException();
        }

        private FunctionEvaluationBase? EvaluateFunction(
            FunctionEvaluationBase functionEvaluationBase)
        {
            return null;
        }
    }
}