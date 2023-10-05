namespace AParser.Tree;

public interface INodeFactory
{
    INode Create(NodeType nodeType);
}