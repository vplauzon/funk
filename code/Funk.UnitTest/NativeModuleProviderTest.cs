using System.Reflection;
using Funk.Runtime;

namespace Funk.UnitTest
{
    public class NativeModuleProviderTest
    {
        [Theory]
        [InlineData("sys", "native type boolean;")]
        [InlineData("sys.arithmetic", "type numerical = integer | real | rational;")]
        public void EmbeddedModuleContentIsAvailable(string moduleName, string expectedContent)
        {
            var provider = new NativeModuleProvider();
            var field = typeof(NativeModuleProvider).GetField(
                "_nativeModuleMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            var modules = Assert.IsAssignableFrom<IDictionary<string, Func<string>>>(field.GetValue(provider));

            Assert.True(modules.TryGetValue(moduleName, out var contentProvider));
            Assert.NotNull(contentProvider);
            Assert.Contains(expectedContent, contentProvider());
        }

        [Fact]
        public void MissingEmbeddedModuleThrows()
        {
            var method = typeof(NativeModuleProvider).GetMethod(
                "GetModuleContent", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(method);

            var exception = Assert.Throws<TargetInvocationException>(
                () => method.Invoke(null, new object[] { "missing" }));
            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }
    }
}
