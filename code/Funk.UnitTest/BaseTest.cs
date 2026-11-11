
using Funk.Runtime;

namespace Funk.UnitTest
{
    public abstract class BaseTest
    {
        protected async Task<string> ToEvaluatedScriptAsync(string scriptText)
        {
            var runtime = new FunkRuntime();
            var expression = await runtime.RunScriptAsync(scriptText);

            throw new NotImplementedException();
        }

        protected async Task<bool> ToBooleanAsync(string script)
        {
            return bool.Parse(await ToEvaluatedScriptAsync(script));
        }

        protected async Task<int> ToIntegerAsync(string script)
        {
            return int.Parse(await ToEvaluatedScriptAsync(script));
        }

        protected async Task<double> ToFloatAsync(string script)
        {
            return double.Parse(await ToEvaluatedScriptAsync(script));
        }

        protected async Task<string> ToStringValueAsync(string script)
        {
            var processedScript = await ToEvaluatedScriptAsync(script);

            if (processedScript.Length >= 2
                &&
                ((processedScript.First() == '\'' && processedScript.Last() == '\'')
                || (processedScript.First() == '"' && processedScript.Last() == '"')))
            {
                return processedScript.Substring(1, processedScript.Length - 2);
            }

            throw new InvalidCastException($"Expression isn't a string:  '{processedScript}'");
        }
    }
}