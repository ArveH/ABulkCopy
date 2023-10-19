namespace AParser.Tree;

public class NodeFactory : INodeFactory
{
    public INode Create(NodeType type)
    {
        return type switch
        {
            NodeType.CommaNode => new CommaNode(),
            NodeType.ConvertFunctionNode => new ConvertFunctionNode(),
            NodeType.LeftParenthesesNode => new LeftParenthesesNode(),
            NodeType.NameNode => new NameNode(),
            NodeType.NStringNode => new NStringNode(),
            NodeType.NumberNode => new NumberNode(),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            NodeType.QuotedNameNode => new QuotedNameNode(),
            NodeType.RightParenthesesNode => new RightParenthesesNode(),
            NodeType.StringNode => new StringNode(),
            NodeType.TodayFunctionNode => new TodayFunctionNode(),
            NodeType.TypeNode => new TypeNode(),
            _ => throw new NodeFactoryException(type)
        };
    }
}