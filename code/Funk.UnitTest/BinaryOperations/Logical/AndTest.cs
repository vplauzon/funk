using System;

namespace Funk.UnitTest.BinaryOperations.Logical
{
    public class AndTest : BaseTest
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void BooleanAnd(bool left, bool right, bool expected)
        {
            var script = $"{left.ToString().ToLower()} && {right.ToString().ToLower()}";
            var primitive = ToBoolean(script);

            Assert.Equal(expected, primitive);
        }
    }
}