using System;

namespace Funk.UnitTest.BinaryOperations.Logical
{
    public class OrTest : BaseTest
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void BooleanOr(bool left, bool right, bool expected)
        {
            var script = $"{left.ToString().ToLower()} || {right.ToString().ToLower()}";
            var primitive = ToBoolean(script);

            Assert.Equal(expected, primitive);
        }
    }
}
