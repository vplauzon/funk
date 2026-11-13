using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    internal class ScriptRunner
    {
        private readonly string? _evaluationText;

        public ScriptRunner(FunctionBodyNode functionBody)
            : this(
                  new NodeFunctionEvaluation(
                      functionBody.FunctionEvaluationStatement.FunctionEvaluation),
                  functionBody.FunctionEvaluationStatement.Text)
        {
        }

        public ScriptRunner(FunctionEvaluationBase functionEvaluation, string evaluationText)
        {
            _evaluationText = evaluationText;
            FunctionEvaluation = functionEvaluation;
        }

        public bool IsFunctionEvaluationOnly => FunctionEvaluation != null;

        public FunctionEvaluationBase? FunctionEvaluation { get; }

        public async Task<ScriptRunner> SimplifyAsync()
        {
            //  Switch the function evaluation to core representation
            var coreEvaluation = FunctionEvaluation!.Visit(
                p => p,
                n => CoreRepresentationFactory.Create(n.FunctionEvaluationNode),
                u => u);

            return new ScriptRunner(coreEvaluation, _evaluationText!);
        }
    }
}