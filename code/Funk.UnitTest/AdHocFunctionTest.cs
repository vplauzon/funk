
namespace Funk.UnitTest
{
    public class AdHocFunctionTest : BaseTest
    {
        [Fact]
        public async Task Substitution()
        {
            var expectedValue = 42;
            var script = @$"
let x = {expectedValue};

x";
            var value = await ToIntegerAsync(script);

            Assert.Equal(expectedValue, value);
        }
    }
}