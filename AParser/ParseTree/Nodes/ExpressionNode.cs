namespace AParser.ParseTree.Nodes;

public class ExpressionNode : INode
{
    public NodeType Type => NodeType.ExpressionNode;
    public bool IsLeafNode => false;
    public List<INode> Children { get; } = new();
}