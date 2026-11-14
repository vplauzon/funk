using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    public class FunkRuntime
    {
        public async Task<FunctionEvaluationBase?> RunScriptAsync(string scriptText)
        {
            var script = ScriptParser.ParseAdHocScript(scriptText);

            if (script.FunctionEvaluation != null)
            {
                var functionLibrary =
                    new FunctionLibrary(script.ImportStatements, script.FunctionDeclarations);
                var functionEvaluationResolver = new FunctionEvaluationResolver(
                    script.FunctionEvaluation.FunctionEvaluation,
                    functionLibrary.GetFunctionLibraryView());
                var simplifiedFunctionEvaluationResolver = await functionEvaluationResolver.SimplifyAsync();

                return simplifiedFunctionEvaluationResolver.FunctionEvaluation;
            }
            else
            {   //  There are no function evaluation in the script
                return null;
            }
        }
    }
}