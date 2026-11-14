using Funk.Runtime.Core;

namespace Funk.Runtime
{
    public class FunctionDefinitionPointer
    {
        private readonly Func<FunctionEvaluationBase> _evaluator;

        public FunctionDefinitionPointer(string fullName, Func<FunctionEvaluationBase> evaluator)
        {
            FullName = fullName;
            _evaluator = evaluator;
        }

        public string FullName { get; }

        public FunctionEvaluationBase Evaluate()
        {
            return _evaluator();
        }
    }
}