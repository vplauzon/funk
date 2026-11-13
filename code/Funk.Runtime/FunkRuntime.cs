using Funk.Parsing;
using Funk.Runtime.Core;

namespace Funk.Runtime
{
    public class FunkRuntime
    {
        public async Task<FunctionEvaluationBase?> RunScriptAsync(string scriptText)
        {
            var script = ScriptParser.ParseAdHocScript(scriptText);

            if (script.FunctionEvaluationStatement != null)
            {
                var functionBody = script.ToFunctionBodyNode();
                var scriptRunner = new ScriptRunner(functionBody);
                var simplifiedScriptRunner = await scriptRunner.SimplifyAsync();

                if (!simplifiedScriptRunner.IsFunctionEvaluationOnly)
                {
                    throw new FunkException("Script couldn't be simplified", scriptText);
                }
                else
                {
                    return simplifiedScriptRunner.FunctionEvaluation;
                }
            }
            else
            {
                return null;
            }
        }
    }
}