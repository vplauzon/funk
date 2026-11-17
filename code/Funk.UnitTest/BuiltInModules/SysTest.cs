namespace Funk.UnitTest.BuiltInModules
{
    public class SysTest : BaseTest
    {
        //[Fact]
        public async Task Booleans()
        {
            var samples = new[] { true, false };

            foreach (var sample in samples)
            {
                var script = $"{sample}";
                var primitive = await ToBooleanAsync(script);

                Assert.Equal(sample, primitive);
            }
        }
    }
}