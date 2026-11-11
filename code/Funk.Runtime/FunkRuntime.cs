using Funk.Parsing;

namespace Funk.Runtime
{
    public class FunkRuntime
    {
        public string Q(string scriptText)
        {
            var script = ScriptParser.ParseScript(scriptText);

            throw new NotImplementedException();
        }
    }
}