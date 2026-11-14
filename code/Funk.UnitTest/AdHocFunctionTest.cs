
namespace Funk.UnitTest
{
    public class AdHocFunctionTest : BaseTest
    {
        [Fact]
        public async Task VariableSubstitution()
        {
            var expectedValue = 42;
            var scripts = new[] {
                $"let x = {expectedValue};x",
                $"let x() = {expectedValue};x",
                $"let x() = {expectedValue};x()",
                $"let x = {expectedValue};x()"
            };

            foreach(var script in scripts)
            {
                var value = await ToIntegerAsync(script);
             
                Assert.Equal(expectedValue, value);
            }
        }
    }
}