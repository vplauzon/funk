using System.Collections.Immutable;

namespace Funk.Runtime
{
    public class NativeModuleProvider
    {
        private readonly IDictionary<string, Func<string>> _nativeModuleMap
            = new Dictionary<string, Func<string>>();

        public NativeModuleProvider()
        {
            AddModule("sys", () => GetModuleContent("sys"));
            AddModule("sys.arithmetic", () => GetModuleContent("sys.arithmetic"));
        }

        public IEnumerable<string> NativeModules => _nativeModuleMap.Keys;

        public void AddModule(string moduleName, Func<string> contentProvider)
        {
            _nativeModuleMap.Add(moduleName, contentProvider);
        }

        private static string GetModuleContent(string moduleName)
        {
            var resourceName = $"{typeof(NativeModuleProvider).Namespace}.Modules.{moduleName}.txt";
            using var stream = typeof(NativeModuleProvider).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Can't find resource file '{resourceName}'");
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
