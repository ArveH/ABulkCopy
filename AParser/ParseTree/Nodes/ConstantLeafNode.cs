namespace AParser.ParseTree.Nodes;

public class ConstantLeafNode : ILeafNode
{
    public ConstantLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.ConstantLeafNode;
    public bool IsLeafNode => true;
    public IToken Token { get; }
}