namespace AParser.ParseTree;

public interface INodeBase
{
    NodeType Type { get; }
    bool IsLeafNode { get; }
}