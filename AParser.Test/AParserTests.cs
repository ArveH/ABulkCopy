namespace AParser.Test
{
    public class AParserTests
    {
        [Fact]
        public void TestAParser()
        {
            IAParser parser = new AParser();

            parser.Should().NotBeNull();
        }
    }
}