namespace AParser.ParseTree.Nodes;

public class SquareLeftParenthesesLeafNode : NodeBase
{
    public SquareLeftParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.SquareLeftParenthesesLeafNode;
    public sealed override IToken? Token { get; set; }
}