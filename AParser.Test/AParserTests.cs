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
            var parser = GetParser();
            
            var action = () => parser.Parse("");

            action.Should().Throw<AParserException>().WithMessage(ErrorMessages.EmptySql);
        }

        [Fact]
        public void TestParseParentheses_When_EmptyParentheses_Then_Exception()
        {
            var parser = GetParser();

            var action = () => parser.Parse("()");

            action.Should().Throw<UnexpectedTokenException>()
                .Where(e => e.ExpectedNodeType == NodeType.ExpressionNode && 
                            e.CurrentTokenName == TokenName.RightParenthesesToken);
        }


        private IAParser GetParser()
        {
            return new AParser(
                new NodeFactory(),
                new Tokenizer(new TokenFactory()));
        }
    }
}