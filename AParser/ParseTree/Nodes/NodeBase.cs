namespace AParser.ParseTree.Nodes;

public abstract class NodeBase : INode
{
    public abstract NodeType Type { get; }
    public bool IsLeafNode => Children == null;
    public virtual List<INode>? Children
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public virtual IToken? Token
    {
        get => null;
        set => throw new NotImplementedException();
    }
}