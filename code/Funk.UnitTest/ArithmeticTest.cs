
namespace Funk.UnitTest
{
    public class ArithmeticTest : BaseTest
    {
        [Fact]
        public void IntegerSum()
        {
            var script = "1 + 2";
            var primitive = ToInteger(script);

            Assert.Equal(3, primitive);
        }
    }
}