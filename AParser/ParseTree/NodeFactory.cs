namespace AParser.ParseTree;

public class NodeFactory : INodeFactory
{
    public INode CreateNode(NodeType nodeType)
    {
        INode node = nodeType switch
        {
            NodeType.ExpressionNode => new ExpressionNode(),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            _ => throw new Exception($"Unknown node type: {nodeType}")
        };

        return node;
    }
}