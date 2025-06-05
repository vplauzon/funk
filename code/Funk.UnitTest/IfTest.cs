
namespace Funk.UnitTest
{
    public class IfTest : BaseTest
    {
        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void TernaryIf(bool condition, int expected)
        {
            var script = $"if({condition.ToString().ToLower()}, 1, 2)";
            var primitive = ToInteger(script);

            Assert.Equal(expected, primitive);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void ChainedIfElse(int input)
        {
            var script = @$"if({input}==1)
{{
    return 2;
}}
else if({input}==2)
{{
return 3;
}}
else if({input}==3)
{{
return 4;
}}
else if({input}==4)
{{
return 5;
}}
else if({input}==5)
{{
return 6;
}}
else
{{
return 7;
}}
";
            var expected = input + 1;
            var primitive = ToInteger(script);

            Assert.Equal(expected, primitive);
        }
    }
}