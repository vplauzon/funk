namespace Funk.UnitTest.BinaryOperations.Inequalities
{
    public class LesserOrEqualTest : BaseTest
    {
        [Theory]
        [InlineData(5.0, 3.0, false)]    // 5 <= 3
        [InlineData(3.0, 5.0, true)]     // 3 <= 5
        [InlineData(5.0, 5.0, true)]     // 5 <= 5
        [InlineData(-2.0, -5.0, false)]  // -2 <= -5
        [InlineData(-5.0, -2.0, true)]   // -5 <= -2
        [InlineData(0.0, -1.0, false)]   // 0 <= -1
        [InlineData(-1.0, 0.0, true)]    // -1 <= 0
        [InlineData(0.0, 0.0, true)]     // 0 <= 0
        public void FloatLesserThanOrEqual(double left, double right, bool expected)
        {
            var script = $"{left} <= {right}";
            var primitive = ToBoolean(script);

            Assert.Equal(expected, primitive);
        }
    }
}
