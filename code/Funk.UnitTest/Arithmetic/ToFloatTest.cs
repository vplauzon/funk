
namespace Funk.UnitTest.Arithmetic
{
    public class ToFloatTest : BaseTest
    {
        [Fact]
        public void FromInteger()
        {
            var samples = new[] { 1, 3, 5, 10, -4, -50 };

            foreach (var sample in samples)
            {
                var script = $"toFloat({sample})";
                var expectedValue = (double)sample;
                var primitive = ToFloat(script);

                Assert.Equal(expectedValue, primitive);
            }
        }

        [Fact]
        public void FromFloat()
        {
            var samples = new[] { 1.5, 3.789, 5.1, 10.2, -4.78, -50.45 };

            foreach (var sample in samples)
            {
                var script = $"toFloat({sample})";
                var expectedValue = sample;
                var primitive = ToFloat(script);

                Assert.Equal(expectedValue, primitive);
            }
        }
    }
}