namespace Funk.UnitTest.Logic
{
    public class InequalityTest : BaseTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task LessThanInteger(int a, int b)
        {
            var script = $"{a}<{b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a < b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task LessOrEqualToInteger(int a, int b)
        {
            var script = $"{a}<={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a <= b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task GreaterThanInteger(int a, int b)
        {
            var script = $"{a}>{b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a > b, primitive);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task GreaterOrEqualToInteger(int a, int b)
        {
            var script = $"{a}>={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a >= b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public async Task LessThanDouble(double a, double b)
        {
            var script = $"{a}<{b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a < b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public async Task LessOrEqualToDouble(double a, double b)
        {
            var script = $"{a}<={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a <= b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public async Task GreaterThanDouble(double a, double b)
        {
            var script = $"{a}>{b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a > b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public async Task GreaterOrEqualToDouble(double a, double b)
        {
            var script = $"{a}>={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a >= b, primitive);
        }
    }
}
