namespace AParser.ParseTree;

public interface INode
{
    NodeType Type { get; }
    bool IsLeafNode { get; }
    List<INode>? Children { get; }
    IToken? Token { get; protected set; }
}