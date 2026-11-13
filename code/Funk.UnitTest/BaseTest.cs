using Funk.Runtime;
using Funk.Runtime.Core;

namespace Funk.UnitTest
{
    public abstract class BaseTest
    {
        protected async Task<FunctionEvaluationBase?> EvaluateScriptAsync(string scriptText)
        {
            var runtime = new FunkRuntime();
            var functionEvaluation = await runtime.RunScriptAsync(scriptText);

            return functionEvaluation;
        }

        protected async Task<object> EvaluateScriptToPrimitiveAsync(string scriptText)
        {
            var functionEvaluation = await EvaluateScriptAsync(scriptText);

            return ((PrimitiveFunctionEvaluation)functionEvaluation!).Value;
        }

        protected async Task<bool> ToBooleanAsync(string script)
        {
            return (bool)await EvaluateScriptToPrimitiveAsync(script);
        }

        protected async Task<long> ToIntegerAsync(string script)
        {
            return (long)await EvaluateScriptToPrimitiveAsync(script);
        }

        protected async Task<double> ToRealAsync(string script)
        {
            return (double)await EvaluateScriptToPrimitiveAsync(script);
        }

        protected async Task<string> ToStringValueAsync(string script)
        {
            return (string)await EvaluateScriptToPrimitiveAsync(script);
        }
    }
}