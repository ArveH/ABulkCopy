namespace AParser.ParseTree.Nodes;

public class LeftParenthesesLeafNode : NodeBase
{
    public LeftParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.LeftParenthesesLeafNode;
    public sealed override IToken? Token { get; set; }
}