
namespace Funk.UnitTest
{
    public class IfTest : BaseTest
    {
        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void IfBranchSelection(bool condition, int expected)
        {
            var script = $"if({condition.ToString().ToLower()}, 1, 2)";
            var primitive = ToInteger(script);

            Assert.Equal(expected, primitive);
        }
    }
}
