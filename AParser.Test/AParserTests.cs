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

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseQuotedName()
        {
            const string testVal = "arve";
            _tokenizer.Initialize($"[{testVal}]");
            _tokenizer.GetNext();

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseNumber()
        {
            const string testVal = "123";
            _tokenizer.Initialize($"[{testVal}]");
            _tokenizer.GetNext();

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestParseParentheses_When_EmptyString_Then_Exception()
        {
            _tokenizer.Initialize("");
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree);

            action.Should().Throw<AParserException>().WithMessage(ErrorMessages.EmptySql);
        }

        [Fact]
        public void TestParseParentheses_When_EmptyParentheses_Then_Exception()
        {
            _tokenizer.Initialize("()");
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree);

            action.Should().Throw<UnexpectedTokenException>()
                .Where(e => e.Expected == null &&
                            e.Current == TokenName.RightParenthesesToken);
        }

        [Fact]
        public void TestParseParentheses_When_ContainsName()
        {
            const string testVal = "(arve)";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            _parser.ParseExpression(_tokenizer, _parseTree);

        }

        [Fact]
        public void TestConvertFunction()
        {
            const string testVal = "(CONVERT([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

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
            return new AParser(new SqlFunctions(), new SqlTypes());
        }
    }
}