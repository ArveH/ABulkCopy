namespace AParser.ParseTree.Nodes;

public class RightParenthesesLeafNode : NodeBase
{
    public RightParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.RightParenthesesLeafNode;
    public sealed override IToken? Token { get; set; }
}