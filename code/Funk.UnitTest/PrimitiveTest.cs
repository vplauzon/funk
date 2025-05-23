
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

        [Fact]
        public void False()
        {
            var script = "false";
            var primitive = ToPrimitive(script);

            Assert.False(primitive.ToBoolean());
        }

        [Fact]
        public void Integers()
        {
            var samples = new[] { 1, 0, -43, 10000 };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = ToPrimitive(script);

                Assert.Equal(sample, primitive.ToInteger());
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
                var primitive = ToPrimitive(script);

                Assert.Equal(double.Parse(sample), primitive.ToFloat());
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
                    var primitive = ToPrimitive(script);

                    Assert.Equal(sample, primitive.ToStringPrimitive());
                }
            }
        }
    }
}