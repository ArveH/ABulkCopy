namespace AParser.ParseTree.Nodes;

public class NameLeafNode : NodeBase
{
    public NameLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.NameLeafNode;
    public sealed override IToken? Token { get; set; }
}