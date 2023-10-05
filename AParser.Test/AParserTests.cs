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

            var node = _parser.ParseName(_tokenizer, _parseTree);

            node.Children.Count.Should().Be(0);
            node.NodeType.Should().Be(NodeType.NameNode);
        }

        [Fact]
        public void TestParseNumber()
        {
            const string testVal = "123";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var node = _parser.ParseNumber(_tokenizer, _parseTree);

            node.Children.Count.Should().Be(0);
            node.NodeType.Should().Be(NodeType.NumberNode);
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

            var node = _parser.ParseParentheses(_tokenizer, _parseTree);

            ValidateParenthesesNode(node);
        }

        [Fact]
        public void TestParseConvertFunction()
        {
            const string testVal = "(CONVERT([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var parenthesesNode = _parser.ParseExpression(_tokenizer, _parseTree);

            ValidateParenthesesNode(parenthesesNode);
            var convertFunctionNode = parenthesesNode.Children[1];
            ValidateConvertFunctionNode(convertFunctionNode);
        }

        [Fact]
        public void TestParseConvertFunction_When_SqlTypeNotQuoted()
        {
            const string testVal = "convert ( bit  ,0 ) ";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var convertFunctionNode = _parser.ParseConvertFunction(_tokenizer, _parseTree);

            ValidateConvertFunctionNode(convertFunctionNode);
        }

        [Fact]
        public void TestParseFunction_When_UnknownFunction()
        {
            const string testVal = "(CAST([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree);

            action.Should().Throw<UnknownFunctionException>()
                .WithMessage(ErrorMessages.UnknownFunction("cast"));
        }

        [Fact]
        public void TestParseFunction_When_UnknownType()
        {
            const string testVal = "(CONVERT([paper],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _parser.ParseExpression(_tokenizer, _parseTree);

            action.Should().Throw<UnknownSqlTypeException>()
                .WithMessage(ErrorMessages.UnknownSqlType(
                    "paper"));
        }

        private static void ValidateParenthesesNode(INode node)
        {
            node.Children.Count.Should().Be(3);
            node.NodeType.Should().Be(NodeType.ParenthesesNode);
            node.Children[0].NodeType.Should().Be(NodeType.LeftParenthesesNode);
            node.Children[2].NodeType.Should().Be(NodeType.RightParenthesesNode);
        }

        private static void ValidateConvertFunctionNode(INode node)
        {
            node.Children.Count.Should().Be(6);
            node.NodeType.Should().Be(NodeType.ConvertFunctionNode);
            node.Children[0].NodeType.Should().Be(NodeType.NameNode);
            node.Children[1].NodeType.Should().Be(NodeType.LeftParenthesesNode);
            node.Children[2].NodeType.Should().Be(NodeType.TypeNode);
            node.Children[3].NodeType.Should().Be(NodeType.CommaNode);
            node.Children[5].NodeType.Should().Be(NodeType.RightParenthesesNode);
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
            return new AParser(new NodeFactory(), new SqlTypes());
        }
    }
}