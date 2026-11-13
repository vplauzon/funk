using GramParserLib;
using System.Reflection;

namespace Funk.Parsing
{
    public static class ScriptParser
    {
        private static readonly Grammar _grammar = CreateGrammar();

        #region Constructor
        private static Grammar CreateGrammar()
        {
            var grammarText = GetResource("Funk-Grammar.txt");
            var grammar = MetaGrammar.ParseGrammar(grammarText);

            if (grammar == null)
            {
                throw new InvalidOperationException("Couldn't create grammar");
            }

            return grammar;
        }

        private static string GetResource(string resourceName)
        {
            var type = typeof(ScriptParser);
            var assembly = type.GetTypeInfo().Assembly;
            var typeNamespace = type.Namespace;
            var fullResourceName = $"{typeNamespace}.{resourceName}";

            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null)
                {
                    throw new ArgumentException(
                        $"Can't find resource file '{resourceName}'",
                        nameof(resourceName));
                }
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();

                    return text;
                }
            }
        }
        #endregion

        public static AdHocScriptNode ParseAdHocScript(string text)
        {
            var match = _grammar!.Match("adHocScript", text);

            if (match == null)
            {
                throw new InvalidOperationException($"Couldn't parse script '{text}'");
            }

            var output = match.ComputeTypedOutput<AdHocScriptNode>(
                ScriptSerializerContext.Default.GetTypeInfo(typeof(AdHocScriptNode))!);

            if (output == null)
            {
                throw new InvalidOperationException(
                    $"Couldn't compute output for script '{text}'");
            }

            return output;
        }
    }
}