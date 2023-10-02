namespace AParser.ParseTree.Nodes;

public class FunctionNode : INode
{
    public NodeType Type => NodeType.FunctionNode;
    public bool IsLeafNode => false;
    public List<INode> Children { get; } = new();
}