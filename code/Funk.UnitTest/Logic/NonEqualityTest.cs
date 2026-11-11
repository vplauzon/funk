namespace Funk.UnitTest.Logic
{
    public class NonEqualityTest : BaseTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task NonEqualityInteger(int a, int b)
        {
            var script = $"{a}!={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a != b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public async Task NonEqualityDouble(double a, double b)
        {
            var script = $"{a}!={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a != b, primitive);
        }

        [Theory]
        [InlineData(1, 1.0)]
        [InlineData(1, 2.0)]
        [InlineData(2, 1.0)]
        [InlineData(2, 2.0)]
        public async Task NonEqualityMixed(int a, double b)
        {
            var script = $"{a}!={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a != b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1)]
        [InlineData(1.0, 2)]
        [InlineData(2.0, 1)]
        [InlineData(2.0, 2)]
        public async Task NonEqualityMixedReverse(double a, int b)
        {
            var script = $"{a}!={b}";
            var primitive = await ToBooleanAsync(script);

            Assert.Equal(a != b, primitive);
        }
    }
}
