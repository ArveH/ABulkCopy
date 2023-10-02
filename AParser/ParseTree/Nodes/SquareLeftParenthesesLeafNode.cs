namespace AParser.ParseTree.Nodes;

public class SquareLeftParenthesesLeafNode : ILeafNode
{
    public SquareLeftParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.SquareLeftParenthesesLeafNode;
    public bool IsLeafNode => true;
    public IToken Token { get; }
}