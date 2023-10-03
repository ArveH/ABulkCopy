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
        public void TestParseName()
        {
            var tokenizer = GetTokenizer();
            var parser = GetParser(tokenizer);
            const string name = "arve";

            var rootNode = parser.Parse(name);

            VerifyExpressionNode(rootNode);
            VerifyNameNode(rootNode.Children![0], name, tokenizer);
        }

        [Fact]
        public void TestParseQuotedName()
        {
            var tokenizer = GetTokenizer();
            var parser = GetParser(tokenizer);
            const string name = "arve";

            var rootNode = parser.Parse($"[{name}]");

            VerifyExpressionNode(rootNode);
            var quotedName = rootNode.Children![0];
            quotedName.Type.Should().Be(NodeType.QuotedNameNode);
            quotedName.Children.Should().HaveCount(3, "because the quoted name node should have 3 children");
            VerifyLeafNode(
                quotedName.Children![0],
                NodeType.SquareLeftParenthesesLeafNode,
                TokenName.SquareLeftParenthesesToken);
            VerifyNameNode(quotedName.Children![1], name, tokenizer);
            VerifyLeafNode(
                quotedName.Children![2],
                NodeType.SquareRightParenthesesLeafNode,
                TokenName.SquareRightParenthesesToken);
        }

        [Fact]
        public void TestParseNumber()
        {
            var tokenizer = GetTokenizer();
            var parser = GetParser(tokenizer);
            const string num = "123";

            var rootNode = parser.Parse(num);

            VerifyExpressionNode(rootNode);
            VerifyNumberNode(rootNode.Children![0], num, tokenizer);
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

        [Fact]
        public void TestParseParentheses_When_ContainsName()
        {
            var tokenizer = GetTokenizer();
            var parser = GetParser(tokenizer);
            const string name = "arve";

            var rootNode = parser.Parse($"({name})");

            VerifyExpressionNode(rootNode);
            var parenthesesNode = rootNode.Children![0];
            parenthesesNode.Type.Should().Be(NodeType.ParenthesesNode);
            parenthesesNode.Children.Should().HaveCount(3, "because the parentheses node should have 3 children");
            VerifyLeafNode(
                parenthesesNode.Children![0], 
                NodeType.LeftParenthesesLeafNode, 
                TokenName.LeftParenthesesToken);
            VerifyLeafNode(
                parenthesesNode.Children![2],
                NodeType.RightParenthesesLeafNode,
                TokenName.RightParenthesesToken);
            var expressionNode = parenthesesNode.Children![1];
            VerifyExpressionNode(expressionNode);
            VerifyNameNode(expressionNode.Children![0], name, tokenizer);
        }

        private static void VerifyExpressionNode(INode expressionNode)
        {
            expressionNode.Should().NotBeNull("because we should have an ExpressionNode");
            expressionNode.Type.Should().Be(NodeType.ExpressionNode);
            expressionNode.Children.Should().NotBeNull("because the ExpressionNode should have one child");
            expressionNode.Children.Should().HaveCount(1, "because the ExpressionNode should have one child");
        }

        private static void VerifyNameNode(INode node, string name, ITokenizer tokenizer)
        {
            VerifyLeafNode(node, NodeType.NameLeafNode, TokenName.NameToken);
            tokenizer.GetSpelling(node.Token!).ToString().Should().Be(name);
        }

        private static void VerifyNumberNode(INode node, string number, ITokenizer tokenizer)
        {
            VerifyLeafNode(node, NodeType.NumberLeafNode, TokenName.NumberToken);
            tokenizer.GetSpelling(node.Token!).ToString().Should().Be(number);
        }

        private static void VerifyLeafNode(
            INode node,
            NodeType expectedNodeType,
            TokenName expectedTokenName)
        {
            node.Type.Should().Be(expectedNodeType);
            node.Token.Should().NotBeNull($"because a {expectedNodeType} should have a token");
            node.Token!.Name.Should().Be(expectedTokenName,
                $"because TokenName for {expectedNodeType} should be {expectedTokenName}");
            node.Children.Should().BeNull($"because a {expectedNodeType} should not have any children");
        }

        private ITokenizer GetTokenizer()
        {
            return new Tokenizer(new TokenFactory());
        }

        private IAParser GetParser(ITokenizer? tokenizer = null)
        {
            return new AParser(
                new NodeFactory(),
                tokenizer ?? GetTokenizer());
        }
    }
}