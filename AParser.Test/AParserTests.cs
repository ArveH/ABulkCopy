namespace AParser.Test
{
    public class AParserTests
    {
        private readonly ITokenizer _tokenizer;
        private readonly IParseTree _parseTree;
        private readonly IAParser _parser;

        public AParserTests()
        {
            _tokenizer = GetTokenizer();
            _parseTree = GetParseTree();
            _parser = GetParser();
        }

        [Fact]
        public void TestAParser_Initialization()
        {
            _parser.Should().NotBeNull();
        }

        [Fact]
        public void TestParseName()
        {
            const string testVal = "arve";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            _parser.ParseName(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseNumber()
        {
            const string testVal = "123";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            _parser.ParseNumber(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseParentheses_When_EmptyParentheses_Then_Exception()
        {
            _tokenizer.Initialize("()");
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree);

            action.Should().Throw<AParserException>()
                .WithMessage(ErrorMessages.UnexpectedToken(TokenType.RightParenthesesToken));
        }

        [Fact]
        public void TestParseExpression_When_ContainsParenthesesAndNumber()
        {
            const string testVal = "(123)";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseConvertFunction()
        {
            const string testVal = "(CONVERT([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        private IParseTree GetParseTree()
        {
            return new ParseTree();
        }

        private ITokenizer GetTokenizer()
        {
            return new Tokenizer(new TokenFactory());
        }

        private IAParser GetParser()
        {
            return new AParser(new SqlTypes());
        }
    }
}