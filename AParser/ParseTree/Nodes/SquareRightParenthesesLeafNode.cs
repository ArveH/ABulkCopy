namespace AParser.ParseTree.Nodes;

public class SquareRightParenthesesLeafNode : NodeBase
{
    public SquareRightParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.SquareRightParenthesesLeafNode;
    public sealed override IToken? Token { get; set; }
}