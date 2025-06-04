using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.UnitTest.BinaryOperations.Arithmetic
{
    public class GreatestCommonDivisorTest : BaseTest
    {
        [Theory]
        [InlineData(15, 25, 5)]
        [InlineData(6, 9, 3)]
        [InlineData(100, 100000, 100)]
        [InlineData(30, 10000, 10)]
        [InlineData(-24, 9, 3)]
        public void Gcd(int a, int b, float gcd)
        {
            var script = $"greatestCommonDivisor({a}, {b})";
            var primitive = ToInteger(script);

            Assert.Equal(gcd, primitive);
        }
    }
}