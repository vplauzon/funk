using System.Collections.Immutable;

namespace Funk.Runtime
{
    public class NativeModuleProvider
    {
        private readonly IDictionary<string, Func<string>> _nativeModuleMap
            = new Dictionary<string, Func<string>>();

        public NativeModuleProvider()
        {
            AddModule("sys");
            AddModule("sys.arithmetric");
        }

        public IImmutableList<string> NativeModules { get; }

        public void AddModule(string moduleName, Func<string> contentProvider)
        {
            _nativeModuleMap.Add(moduleName, contentProvider);
        }
    }
}
