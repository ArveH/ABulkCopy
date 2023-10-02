namespace AParser.ParseTree.Nodes;

public class TypeLeafNode : ILeafNode
{
    public TypeLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.TypeLeafNode;
    public bool IsLeafNode => true;
    public IToken Token { get; }
}