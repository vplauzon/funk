using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class FunctionEvaluationResolver
    {
        private readonly IFunctionLibrary _parentLibrary;

        public FunctionEvaluationResolver(
            FunctionEvaluationNode functionEvaluationNode,
            IFunctionLibrary parentLibrary)
            : this(new NodeFunctionEvaluation(functionEvaluationNode), parentLibrary)
        {
        }

        private FunctionEvaluationResolver(
            FunctionEvaluationBase functionEvaluation,
            IFunctionLibrary parentLibrary)
        {
            FunctionEvaluation = functionEvaluation;
            _parentLibrary = parentLibrary;
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

            return new FunctionEvaluationResolver(coreEvaluation, _parentLibrary);
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