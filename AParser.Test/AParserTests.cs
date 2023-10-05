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

            var tokens = _parser.ParseName(_tokenizer, _parseTree).ToList();

            tokens.Count.Should().Be(1);
            tokens[0].Type.Should().Be(TokenType.NameToken);
        }

        [Fact]
        public void TestParseNumber()
        {
            const string testVal = "123";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var tokens = _parser.ParseNumber(_tokenizer, _parseTree).ToList();

            tokens.Count.Should().Be(1);
            tokens[0].Type.Should().Be(TokenType.NumberToken);
        }

        [Fact]
        public void TestParseParentheses_When_EmptyParentheses_Then_Exception()
        {
            _tokenizer.Initialize("()");
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            action.Should().Throw<AParserException>()
                .WithMessage(ErrorMessages.UnexpectedToken(TokenType.RightParenthesesToken));
        }

        [Fact]
        public void TestParseExpression_When_ContainsParenthesesAndNumber()
        {
            const string testVal = "(123)";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var tokens = _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            tokens.Count.Should().Be(3);
            tokens[0].Type.Should().Be(TokenType.LeftParenthesesToken);
            tokens[1].Type.Should().Be(TokenType.NumberToken);
            tokens[2].Type.Should().Be(TokenType.RightParenthesesToken);
        }

        [Fact]
        public void TestParseConvertFunction()
        {
            const string testVal = "(CONVERT([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var tokens = _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            tokens.Count.Should().Be(10);
            tokens[0].Type.Should().Be(TokenType.LeftParenthesesToken);
            tokens[1].Type.Should().Be(TokenType.NameToken);
            tokens[2].Type.Should().Be(TokenType.LeftParenthesesToken);
            tokens[3].Type.Should().Be(TokenType.QuotedNameToken);
            tokens[4].Type.Should().Be(TokenType.CommaToken);
            tokens[5].Type.Should().Be(TokenType.LeftParenthesesToken);
            tokens[6].Type.Should().Be(TokenType.NumberToken);
            tokens[7].Type.Should().Be(TokenType.RightParenthesesToken);
            tokens[8].Type.Should().Be(TokenType.RightParenthesesToken);
            tokens[9].Type.Should().Be(TokenType.RightParenthesesToken);
        }

        [Fact]
        public void TestParseConvertFunction_When_SqlTypeNotQuoted()
        {
            const string testVal = "convert ( bit  ,0 ) ";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var tokens = _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            tokens.Count.Should().Be(6);
            tokens[0].Type.Should().Be(TokenType.NameToken);
            tokens[1].Type.Should().Be(TokenType.LeftParenthesesToken);
            tokens[2].Type.Should().Be(TokenType.NameToken);
            tokens[3].Type.Should().Be(TokenType.CommaToken);
            tokens[4].Type.Should().Be(TokenType.NumberToken);
            tokens[5].Type.Should().Be(TokenType.RightParenthesesToken);
        }

        [Fact]
        public void TestParseFunction_When_UnknownFunction()
        {
            const string testVal = "(CAST([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            action.Should().Throw<UnknownFunctionException>()
                .WithMessage(ErrorMessages.UnknownFunction("cast"));
        }

        [Fact]
        public void TestParseFunction_When_UnknownType()
        {
            const string testVal = "(CONVERT([paper],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree).ToList();

            action.Should().Throw<UnknownSqlTypeException>()
                .WithMessage(ErrorMessages.UnknownSqlType(
                    "paper"));
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