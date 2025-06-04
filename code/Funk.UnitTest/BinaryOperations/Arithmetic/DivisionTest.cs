
namespace Funk.UnitTest.BinaryOperations.Arithmetic
{
    public class DivisionTest : BaseTest
    {
        [Fact]
        public void FloatDivision()
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
                var script = $"{sample.Item1} / {sample.Item2}";
                var expectedValue = sample.Item1 / sample.Item2;
                var primitive = ToFloat(script);

                Assert.Equal(expectedValue, primitive);
            }
        }
    }
}