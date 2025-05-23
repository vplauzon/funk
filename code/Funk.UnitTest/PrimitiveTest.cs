
namespace Funk.UnitTest
{
    public class PrimitiveTest : BaseTest
    {
        [Fact]
        public void True()
        {
            var script = "true";
            var primitive = ToPrimitive(script);

            Assert.True(primitive.ToBoolean());
        }
    }
}