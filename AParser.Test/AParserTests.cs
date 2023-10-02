namespace AParser.Test
{
    public class AParserTests
    {
        [Fact]
        public void TestAParser_Initialization()
        {
            var parser = GetParser();
            parser.Should().NotBeNull();
        }

        [Fact]
        public void TestParseParentheses_When_EmptyString_Then_Exception()
        {
            IAParser parser = GetParser();
            
            var action = () => parser.Parse("");

            action.Should().Throw<AParserException>().WithMessage(ErrorMessages.EmptySql);
        }

        private IAParser GetParser()
        {
            return new AParser(new Tokenizer(new TokenFactory()));
        }
    }
}