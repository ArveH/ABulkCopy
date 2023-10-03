namespace AParser.ParseTree.Nodes;

public class ConstantLeafNode : NodeBase
{
    public ConstantLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.ConstantLeafNode;
    public sealed override IToken? Token { get; set; }
}