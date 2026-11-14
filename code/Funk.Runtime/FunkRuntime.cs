using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    /// <summary>
    /// Holds different caches so should be used as a singleton but isn't thread safe.
    /// </summary>
    public class FunkRuntime
    {
        private readonly NativeModuleProvider _nativeModuleProvider = new();

        public async Task<FunctionEvaluationBase?> RunScriptAsync(string scriptText)
        {
            var script = ScriptParser.ParseAdHocScript(scriptText);

            if (script.FunctionEvaluation != null)
            {
                var functionLibrary = new FunctionLibrary(
                    _nativeModuleProvider.NativeModules,
                    script.ImportStatements,
                    script.FunctionDeclarations);
                var functionEvaluationResolver = new FunctionEvaluationResolver(
                    script.FunctionEvaluation.FunctionEvaluation,
                    functionLibrary.GetFunctionLibraryView());
                var simplifiedFunctionEvaluationResolver = functionEvaluationResolver.Simplify();

                return simplifiedFunctionEvaluationResolver.FunctionEvaluation;
            }
            else
            {   //  There are no function evaluation in the script
                return null;
            }
        }
    }
}