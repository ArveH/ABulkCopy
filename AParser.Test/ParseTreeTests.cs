namespace AParser.Test
{
    public class ParseTreeTests : ParserTestBase
    {
        private readonly ITokenizer _tokenizer;
        private readonly IParseTree _tree;

        public ParseTreeTests()
        {
            _tokenizer = GetTokenizer();
            _tree = GetParseTree();
        }

        [Fact]
        public void TestCreateTree_Initialization()
        {
            _tree.Should().NotBeNull();
        }

        [Fact]
        public void TestCreateName()
        {
            const string testVal = "arve";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var node = _tree.CreateName(_tokenizer);

            node.Children.Count.Should().Be(0);
            node.Type.Should().Be(NodeType.NameNode);
        }

        [Fact]
        public void TestCreateNumber()
        {
            const string testVal = "123";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var node = _tree.CreateNumber(_tokenizer);

            node.Children.Count.Should().Be(0);
            node.Type.Should().Be(NodeType.NumberNode);
        }

        [Fact]
        public void TestCreateParentheses_When_EmptyParentheses_Then_Exception()
        {
            _tokenizer.Initialize("()");
            _tokenizer.GetNext();

            var action = () => _tree.CreateExpression(_tokenizer);

            action.Should().Throw<AParserException>()
                .WithMessage(ErrorMessages.UnexpectedToken(TokenType.RightParenthesesToken));
        }

        [Fact]
        public void TestCreateParentheses_When_ContainsParenthesesAndNumber()
        {
            const string testVal = "(123)";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var node = _tree.CreateParentheses(_tokenizer);

            ValidateParenthesesNode(node);
        }

        [Fact]
        public void TestCreateExpression_When_ConvertFunction()
        {
            const string testVal = "(CONVERT([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var parenthesesNode = _tree.CreateExpression(_tokenizer);

            ValidateParenthesesNode(parenthesesNode);
            var convertFunctionNode = parenthesesNode.Children[1];
            ValidateConvertFunctionNode(convertFunctionNode);
        }

        [Fact]
        public void TestCreateConvertFunction_When_SqlTypeNotQuoted()
        {
            const string testVal = "convert ( bit  ,0 ) ";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var convertFunctionNode = _tree.CreateConvertFunction(_tokenizer);

            ValidateConvertFunctionNode(convertFunctionNode);
        }

        [Fact]
        public void TestCreateExpression_When_UnknownFunction()
        {
            const string testVal = "(CAST([bit],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _tree.CreateExpression(_tokenizer);

            action.Should().Throw<UnknownFunctionException>()
                .WithMessage(ErrorMessages.UnknownFunction("cast"));
        }

        [Fact]
        public void TestCreateExpression_When_UnknownType()
        {
            const string testVal = "(CONVERT([paper],(0)))";
            _tokenizer.Initialize(testVal);
            _tokenizer.GetNext();

            var action = () => _tree.CreateExpression(_tokenizer);

            action.Should().Throw<UnknownSqlTypeException>()
                .WithMessage(ErrorMessages.UnknownSqlType(
                    "paper"));
        }

        private static void ValidateParenthesesNode(INode node)
        {
            node.Children.Count.Should().Be(3);
            node.Type.Should().Be(NodeType.ParenthesesNode);
            node.Children[0].Type.Should().Be(NodeType.LeftParenthesesNode);
            node.Children[2].Type.Should().Be(NodeType.RightParenthesesNode);
        }

        private static void ValidateConvertFunctionNode(INode node)
        {
            node.Children.Count.Should().Be(6);
            node.Type.Should().Be(NodeType.ConvertFunctionNode);
            node.Children[0].Type.Should().Be(NodeType.NameNode);
            node.Children[1].Type.Should().Be(NodeType.LeftParenthesesNode);
            node.Children[2].Type.Should().Be(NodeType.TypeNode);
            node.Children[3].Type.Should().Be(NodeType.CommaNode);
            node.Children[5].Type.Should().Be(NodeType.RightParenthesesNode);
        }
    }
}