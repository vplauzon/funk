
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

        [Fact]
        public void RationalToInteger()
        {
            var samples = new[]
            {
                (2,1),
                (4,2),
                (6,2),
                (9,3),
                (-15,3),
                (-15,-5)
            };

            foreach (var sample in samples)
            {
                var script = $"{sample.Item1}/{sample.Item2}";
                var expectedValue = sample.Item1 / sample.Item2;
                var primitive = ToInteger(script);

                Assert.Equal(expectedValue, primitive);
            }
        }

        [Fact]
        public void SumToRational()
        {
            var samples = new[]
            {
                ((1,2),(1,4),(3,4)),
                ((-1,8),(1,4),(3,4)),
            };

            foreach (var sample in samples)
            {
                var sumScript = $"{sample.Item1.Item1}/{sample.Item1.Item2}" +
                    $"+{sample.Item2.Item1}/{sample.Item2.Item2}";
                var numeratorScript = $"param({sumScript}, 0)";
                var denumeratorScript = $"param({sumScript}, 1)";
                var numerator = ToInteger(numeratorScript);
                var denumerator = ToInteger(denumeratorScript);

                Assert.Equal(sample.Item3.Item1, numerator);
                Assert.Equal(sample.Item3.Item2, denumerator);
            }
        }

        [Fact]
        public void SumToInteger()
        {
            var samples = new[]
            {
                (34,35,1,35),
                (1,2,1,2),
                (1,4,3,4),
                (-1,4,2,8)
            };

            foreach (var sample in samples)
            {
                var script = $"{sample.Item1}/{sample.Item2}+{sample.Item3}/{sample.Item4}";
                var expectedValue =
                    ((sample.Item1 * sample.Item4) + (sample.Item3 * sample.Item2))
                    / (sample.Item2 * sample.Item4);
                var primitive = ToInteger(script);

                Assert.Equal(expectedValue, primitive);
            }
        }
    }
}