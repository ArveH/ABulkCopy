namespace AParser.Test
{
    public class AParserTests
    {
        [Fact]
        public void TestAParser_Initialization()
        {
            IAParser parser = new AParser();
            parser.Should().NotBeNull();
        }

        [Fact]
        public void TestParseParentheses_When_EmptyString_Then_Exception()
        {
            IAParser parser = new AParser();
            
            var action = () => parser.Parse("");

            action.Should().Throw<UnexpectedTokenException>();
        }
    }
}