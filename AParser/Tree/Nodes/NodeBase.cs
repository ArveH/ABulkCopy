namespace AParser.Tree.Nodes;

public abstract class NodeBase : INode
{
    public abstract NodeType Type { get; }
    public List<IToken> Tokens { get; } = new();
    public List<INode> Children { get; } = new();

}