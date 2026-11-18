
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

            await TestSubstitutionAsync(scripts, expectedValue);
        }

        [Fact]
        public async Task TwoSubstitutions()
        {
            var expectedValue = 42;
            var scripts = new[] {
                $"let x = {expectedValue};let y= x;y",
                $"let x() = {expectedValue};let y= x;y",
                $"let x() = {expectedValue};let y= x;y()",
                $"let x() = {expectedValue};let y()= x;y()",
                $"let x() = {expectedValue};let y()= x();y()",
                $"let x = {expectedValue};let y= x;y()"
            };

            await TestSubstitutionAsync(scripts, expectedValue);
        }

        [Fact]
        public async Task ThreeSubstitutions()
        {
            var expectedValue = 42;
            var scripts = new[] {
                $"let x = {expectedValue};let y= x;let z=y;z",
                $"let x() = {expectedValue};let y= x;let z=y;z",
                $"let x() = {expectedValue};let y= x;let z=y;z()",
                $"let x() = {expectedValue};let y()= x;let z=y;z()",
                $"let x() = {expectedValue};let y()= x();let z=y;z()",
                $"let x = {expectedValue};let y= x;y()"
            };

            await TestSubstitutionAsync(scripts, expectedValue);
        }

        private async Task TestSubstitutionAsync(IEnumerable<string> scripts, long expectedValue)
        {
            foreach (var script in scripts)
            {
                var value = await ToIntegerAsync(script);

                Assert.Equal(expectedValue, value);
            }
        }
    }
}