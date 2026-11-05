
namespace Funk.UnitTest
{
    public class PrimitiveTest : BaseTest
    {
        [Fact]
        public void Booleans()
        {
            var samples = new[] { true, false };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = ToBoolean(script);

                Assert.Equal(sample, primitive);
            }
        }

        [Fact]
        public void Integers()
        {
            var samples = new[] { 1, 0, -43, 10000 };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = ToInteger(script);

                Assert.Equal(sample, primitive);
            }
        }

        [Fact]
        public void Floats()
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
                var primitive = ToFloat(script);

                Assert.Equal(double.Parse(sample), primitive);
            }
        }

        [Fact]
        public void Strings()
        {
            var samples = new[] { "a", "b", "12", "hello", "Yeah!  I like that." };

            foreach (var sample in samples)
            {
                foreach (var quote in new[] { "\"", "'" })
                {
                    var script = $"{quote}{sample}{quote}";
                    var primitive = ToStringValue(script);

                    Assert.Equal(sample, primitive);
                }
            }
        }
    }
}