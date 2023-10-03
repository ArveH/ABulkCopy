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

        [Fact]
        public void TestParseName()
        {
            var tokenizer = new Tokenizer(new TokenFactory());
            var parser = GetParser(tokenizer);
            const string name = "arve";

            var rootNode = parser.Parse(name);

            VerifyRootNode(rootNode);
            VerifyNameNode(rootNode.Children![0], name, tokenizer);
        }

        [Fact]
        public void TestParseQuotedName()
        {
            var tokenizer = new Tokenizer(new TokenFactory());
            var parser = GetParser(tokenizer);
            const string name = "arve";

            var rootNode = parser.Parse($"[{name}]");

            VerifyRootNode(rootNode);
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

        private static void VerifyRootNode(INode rootNode)
        {
            rootNode.Should().NotBeNull("because we should have a root node");
            rootNode.Type.Should().Be(NodeType.ExpressionNode);
            rootNode.Children.Should().NotBeNull("because the root node should have one child");
            rootNode.Children.Should().HaveCount(1, "because the root node should have one child");
        }

        private static void VerifyNameNode(INode node, string name, Tokenizer tokenizer)
        {
            node.Type.Should().Be(NodeType.NameLeafNode);
            node.Token.Should().NotBeNull("because a NameLeafNode should have a token");
            node.Token!.Name.Should().Be(TokenName.NameToken);
            node.Children.Should().BeNull("because a NameLeafNode should not have any children");
            tokenizer.GetSpelling(node.Token).ToString().Should().Be(name);
        }

        private static void VerifyLeafNode(
            INode node, NodeType expectedNodeType, TokenName expectedTokenName)
        {
            node.Type.Should().Be(expectedNodeType);
            node.Token.Should().NotBeNull($"because a {expectedNodeType} should have a token");
            node.Token!.Name.Should().Be(expectedTokenName, $"because TokenName for {expectedNodeType} should be {expectedTokenName}");
            node.Children.Should().BeNull($"because a {expectedNodeType} should not have any children");
        }

        private IAParser GetParser(ITokenizer? tokenizer = null)
        {
            return new AParser(
                new NodeFactory(),
                tokenizer ?? new Tokenizer(new TokenFactory()));
        }
    }
}