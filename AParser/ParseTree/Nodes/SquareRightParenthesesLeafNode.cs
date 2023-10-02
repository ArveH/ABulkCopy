namespace AParser.ParseTree.Nodes;

public class SquareRightParenthesesLeafNode : ILeafNode
{
    public SquareRightParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.SquareRightParenthesesLeafNode;
    public bool IsLeafNode => true;
    public IToken Token { get; }
}