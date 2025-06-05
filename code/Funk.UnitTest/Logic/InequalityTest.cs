namespace Funk.UnitTest.Logic
{
    public class InequalityTest : BaseTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void LessThanInteger(int a, int b)
        {
            var script = $"{a}<{b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a < b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void LessOrEqualToInteger(int a, int b)
        {
            var script = $"{a}<={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a <= b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void GreaterThanInteger(int a, int b)
        {
            var script = $"{a}>{b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a > b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void GreaterOrEqualToInteger(int a, int b)
        {
            var script = $"{a}>={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a >= b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public void LessThanDouble(double a, double b)
        {
            var script = $"{a}<{b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a < b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public void LessOrEqualToDouble(double a, double b)
        {
            var script = $"{a}<={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a <= b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public void GreaterThanDouble(double a, double b)
        {
            var script = $"{a}>{b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a > b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public void GreaterOrEqualToDouble(double a, double b)
        {
            var script = $"{a}>={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a >= b, primitive);
        }
    }
}
