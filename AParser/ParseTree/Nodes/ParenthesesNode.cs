namespace AParser.ParseTree.Nodes;

public class ParenthesesNode : INode
{
    public NodeType Type => NodeType.ParenthesesNode;
    public bool IsLeafNode => false;
    public List<INode> Children { get; } = new();
}