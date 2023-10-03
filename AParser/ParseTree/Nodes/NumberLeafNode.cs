namespace AParser.ParseTree.Nodes;

public class NumberLeafNode : NodeBase
{
    public NumberLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.NumberLeafNode;
    public sealed override IToken? Token { get; set; }
}