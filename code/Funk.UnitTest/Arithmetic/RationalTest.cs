
namespace Funk.UnitTest.Arithmetic
{
    public class RationalTest : BaseTest
    {
        [Fact]
        public void RationalToFloat()
        {
            var samples = new[]
            {
                (1,2),
                (10, 25),
                (-500, 200),
                (-25, -12)
            };

            foreach (var sample in samples)
            {
                var script = $"toFloat({sample.Item1}/{sample.Item2})";
                var expectedValue = (float)sample.Item1 / sample.Item2;
                var primitive = ToFloat(script);

                Assert.True(Math.Abs(expectedValue - primitive) < 0.000001);
            }
        }
    }
}