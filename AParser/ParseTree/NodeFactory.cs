namespace AParser.ParseTree;

public class NodeFactory : INodeFactory
{
    public INode CreateNode(NodeType nodeType, IToken? token = null)
    {
        return nodeType switch
        {
            NodeType.CommaLeafNode => new CommaLeafNode(token ?? throw new TokenArgumentNullException(TokenName.CommaToken)),
            NodeType.ConstantLeafNode => new ConstantLeafNode(token ?? throw new TokenArgumentNullException("constant token")),
            NodeType.ConvertFunctionNode => new FunctionNode(),
            NodeType.ExpressionNode => new ExpressionNode(),
            NodeType.FunctionNode => new FunctionNode(),
            NodeType.LeftParenthesesLeafNode => new LeftParenthesesLeafNode(token ?? throw new TokenArgumentNullException(TokenName.LeftParenthesesToken)),
            NodeType.NameLeafNode => new NameLeafNode(token ?? throw new TokenArgumentNullException("name token")),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            NodeType.QuotedNameNode => new QuotedNameNode(),
            NodeType.RightParenthesesLeafNode => new RightParenthesesLeafNode(token ?? throw new TokenArgumentNullException(TokenName.RightParenthesesToken)),
            NodeType.SquareLeftParenthesesLeafNode => new SquareLeftParenthesesLeafNode(token ?? throw new TokenArgumentNullException(TokenName.SquareLeftParenthesesToken)),
            NodeType.SquareRightParenthesesLeafNode => new SquareRightParenthesesLeafNode(token ?? throw new TokenArgumentNullException(TokenName.SquareRightParenthesesToken)),
            NodeType.TypeLeafNode => new TypeLeafNode(token ?? throw new TokenArgumentNullException("type token")),
            _ => throw new NodeFactoryException(nodeType)
        };
    }
}