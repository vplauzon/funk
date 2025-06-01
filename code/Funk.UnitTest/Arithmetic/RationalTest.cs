
namespace Funk.UnitTest.Arithmetic
{
    using System;

    public class RationalTest : BaseTest
    {
        [Theory]
        [InlineData(1, 2, 0.5f)]
        [InlineData(10, 25, 0.4f)]
        [InlineData(-500, 200, -2.5f)]
        [InlineData(-25, -12, 2.083333f)]
        public void RationalToFloat(int numerator, int denominator, float expected)
        {
            var script = $"toFloat({numerator}/{denominator})";
            var primitive = ToFloat(script);
            Assert.True(Math.Abs(expected - primitive) < 0.000001);
        }

        [Theory]
        [InlineData(2, 1, 2)]
        [InlineData(4, 2, 2)]
        [InlineData(6, 2, 3)]
        [InlineData(9, 3, 3)]
        [InlineData(-15, 3, -5)]
        [InlineData(-15, -5, 3)]
        public void RationalToInteger(int numerator, int denominator, int expected)
        {
            var script = $"{numerator}/{denominator}";
            var primitive = ToInteger(script);
            Assert.Equal(expected, primitive);
        }

        [Theory]
        [InlineData(1, 2, 1, 4, 3, 4)]  // 1/2 + 1/4 = 3/4
        [InlineData(-1, 8, 1, 4, 1, 8)] // -1/8 + 1/4 = 1/8
        [InlineData(1, 8, 1, 8, 1, 4)]  // 1/8 + 1/8 = 1/4
        public void SumToRational(int n1, int d1, int n2, int d2, int expectedNum, int expectedDen)
        {
            var sumScript = $"{n1}/{d1}+{n2}/{d2}";
            var numeratorScript = $"param({sumScript}, 0)";
            var denumeratorScript = $"param({sumScript}, 1)";
            
            var numerator = ToInteger(numeratorScript);
            var denumerator = ToInteger(denumeratorScript);

            Assert.Equal(expectedNum, numerator);
            Assert.Equal(expectedDen, denumerator);
        }

        [Theory]
        [InlineData(34, 35, 1, 35, 1)]  // 34/35 + 1/35 = 1
        [InlineData(1, 2, 1, 2, 1)]     // 1/2 + 1/2 = 1
        [InlineData(1, 4, 3, 4, 1)]     // 1/4 + 3/4 = 1
        [InlineData(-1, 4, 2, 8, 0)]    // -1/4 + 2/8 = 0
        public void SumToInteger(int n1, int d1, int n2, int d2, int expected)
        {
            var script = $"{n1}/{d1}+{n2}/{d2}";
            var primitive = ToInteger(script);
            Assert.Equal(expected, primitive);
        }
    }
}
