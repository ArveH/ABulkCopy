namespace AParser.ParseTree;

public interface INodeFactory
{
    INode CreateNode(NodeType nodeType, IToken? token = null);
}