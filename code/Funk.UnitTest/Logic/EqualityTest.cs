namespace Funk.UnitTest.Logic
{
    public class EqualityTest : BaseTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void EqualityInteger(int a, int b)
        {
            var script = $"{a}=={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a == b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 2.0)]
        [InlineData(2.0, 1.0)]
        [InlineData(2.0, 2.0)]
        public void EqualityDouble(double a, double b)
        {
            var script = $"{a}=={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a == b, primitive);
        }

        [Theory]
        [InlineData(1, 1.0)]
        [InlineData(1, 2.0)]
        [InlineData(2, 1.0)]
        [InlineData(2, 2.0)]
        public void EqualityMixed(int a, double b)
        {
            var script = $"{a}=={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a == b, primitive);
        }

        [Theory]
        [InlineData(1.0, 1)]
        [InlineData(1.0, 2)]
        [InlineData(2.0, 1)]
        [InlineData(2.0, 2)]
        public void EqualityMixedReverse(double a, int b)
        {
            var script = $"{a}=={b}";
            var primitive = ToBoolean(script);

            Assert.Equal(a == b, primitive);
        }
    }
}
