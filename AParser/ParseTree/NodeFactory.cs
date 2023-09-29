namespace AParser.ParseTree;

public class NodeFactory : INodeFactory
{
    public INode CreateNode(NodeType nodeType, ITokenizer tokenizer)
    {
        INode node = nodeType switch
        {
            NodeType.ExpressionNode => new ExpressionNode(),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            _ => throw new Exception($"Unknown node type: {nodeType}")
        };

        node.Create(tokenizer);
        return node;
    }

    public INode CreateLeafNode(NodeType nodeType, ITokenizer tokenizer)
    {
        INode node = nodeType switch
        {
            NodeType.ExpressionNode => new ExpressionNode(),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            _ => throw new Exception($"Unknown node type: {nodeType}")
        };

        node.Create(tokenizer);
        return node;
    }
}