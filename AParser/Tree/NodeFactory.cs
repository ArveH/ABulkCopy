namespace AParser.Tree;

public class NodeFactory : INodeFactory
{
    public INode Create(NodeType nodeType)
    {
        return nodeType switch
        {
            NodeType.CommaNode => new CommaNode(),
            NodeType.ConvertFunctionNode => new ConvertFunctionNode(),
            NodeType.LeftParenthesesNode => new LeftParenthesesNode(),
            NodeType.NameNode => new NameNode(),
            NodeType.NumberNode => new NumberNode(),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            NodeType.QuotedNameNode => new QuotedNameNode(),
            NodeType.RightParenthesesNode => new RightParenthesesNode(),
            NodeType.TypeNode => new TypeNode(),
            _ => throw new NodeFactoryException(nodeType)
        };
    }
}