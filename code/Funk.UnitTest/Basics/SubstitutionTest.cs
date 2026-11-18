
namespace Funk.UnitTest.Basics
{
    public class SubstitutionTest : BaseTest
    {
        [Fact]
        public async Task OneSubstitution()
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