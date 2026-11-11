using Funk.Parsing;

namespace Funk.Runtime
{
    public class FunkRuntime
    {
        public async Task<string> RunScriptAsync(string scriptText)
        {
            await Task.CompletedTask;

            var script = ScriptParser.ParseScript(scriptText);

            throw new NotImplementedException();
        }
    }
}