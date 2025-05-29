
namespace Funk.UnitTest
{
    public class ArithmeticTest : BaseTest
    {
        [Fact]
        public void IntegerSum()
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
                var script = $"{sample.Item1} + {sample.Item2}";
                var expectedValue = sample.Item1 + sample.Item2;
                var primitive = ToInteger(script);

                Assert.Equal(expectedValue, primitive);
            }
        }
    }
}