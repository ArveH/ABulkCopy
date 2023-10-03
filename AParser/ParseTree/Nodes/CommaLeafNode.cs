namespace AParser.ParseTree.Nodes;

public class CommaLeafNode : NodeBase
{
    public CommaLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.CommaLeafNode;
    public sealed override IToken? Token { get; set; }
}
