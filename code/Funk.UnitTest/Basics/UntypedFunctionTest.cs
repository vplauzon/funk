
namespace Funk.UnitTest.Basics
{
    public class UntypedFunctionTest : BaseTest
    {
        //[Fact]
        public async Task Integers()
        {
            var script = $"let f(x, y) = x;f(42,51)";
            var result = await ToIntegerAsync(script);

            Assert.Equal(42, result);
        }
    }
}