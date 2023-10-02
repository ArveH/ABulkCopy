namespace AParser.ParseTree.Nodes;

public class LeftParenthesesLeafNode : ILeafNode
{
    public LeftParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.LeftParenthesesLeafNode;
    public bool IsLeafNode => true;
    public IToken Token { get; }
}