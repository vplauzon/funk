using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class FunctionEvaluationResolver
    {
        private readonly IFunctionLibrary _parentLibrary;

        #region Constructors
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
        #endregion

        public FunctionEvaluationBase FunctionEvaluation { get; }

        public FunctionEvaluationResolver Simplify()
        {
            //  Switch the function evaluation to core representation
            var coreEvaluation = FunctionEvaluation.Visit(
                p => p,
                n => CoreRepresentationFactory.Create(n.FunctionEvaluationNode),
                u => u,
                b => b);
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
            FunctionEvaluationBase? ResolveUnboundFunction(UnboundFunctionEvaluation evaluation)
            {
                var functionDefinition =
                    _parentLibrary.GetFunctionDefinition(evaluation.FunctionName);

                return new BoundFunctionEvaluation(functionDefinition);
            }

            var resolvedEvaluation = evaluation.Visit(
                p => null,
                n => null,
                u => ResolveUnboundFunction(u),
                b => null);

            return resolvedEvaluation;
        }


        private FunctionEvaluationBase? EvaluateFunction(FunctionEvaluationBase evaluation)
        {
            FunctionEvaluationBase? EvaluateBoundFunction(BoundFunctionEvaluation boundFunction)
            {
                return boundFunction.FunctionDefinition.Evaluate();
            }

            var evaluatedFunction = evaluation.Visit(
                p => null,
                n => null,
                u => null,
                b => EvaluateBoundFunction(b));

            return evaluatedFunction;
        }

    }
}