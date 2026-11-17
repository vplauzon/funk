
namespace Funk.UnitTest.Basics
{
    public class PrimitiveTest : BaseTest
    {
        [Fact]
        public async Task Integers()
        {
            var samples = new[] { 1, 0, -43, 10000 };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = await ToIntegerAsync(script);

                Assert.Equal(sample, primitive);
            }
        }

        [Fact]
        public async Task Reals()
        {
            var samples = new[]
            {
                "1.0",
                "0.0",
                "-43.0",
                "10000.0",
                "0.5",
                "489.123",
                "-43.0001"
            };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = await ToRealAsync(script);

                Assert.Equal(double.Parse(sample), primitive);
            }
        }

        [Fact]
        public async Task Strings()
        {
            var samples = new[] { "a", "b", "12", "hello", "Yeah!  I like that." };

            foreach (var sample in samples)
            {
                foreach (var quote in new[] { "\"", "'" })
                {
                    var script = $"{quote}{sample}{quote}";
                    var primitive = await ToStringValueAsync(script);

                    Assert.Equal(sample, primitive);
                }
            }
        }
    }
}