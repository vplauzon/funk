
namespace Funk.UnitTest.BinaryOperations.Arithmetic
{
    public class SumTest : BaseTest
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

        [Fact]
        public void FloatSum()
        {
            var samples = new[]
            {
                (1.5,2.2),
                (10.26, 25.89),
                (-500.12, 200.54),
                (-0.658, 0.54),
                (-25.982, -12.5)
            };

            foreach (var sample in samples)
            {
                var script = $"{sample.Item1} + {sample.Item2}";
                var expectedValue = sample.Item1 + sample.Item2;
                var primitive = ToFloat(script);

                Assert.Equal(expectedValue, primitive);
            }
        }
    }
}
