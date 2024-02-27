namespace AParser.Tree;

public interface INode
{
    public NodeType Type { get; }
    public List<IToken> Tokens { get; }
    public List<INode> Children { get; }
}