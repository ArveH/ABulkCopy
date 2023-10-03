namespace AParser.ParseTree.Nodes;

public class TypeLeafNode : NodeBase
{
    public TypeLeafNode(IToken token)
    {
        Token = token;
    }

    public override NodeType Type => NodeType.TypeLeafNode;
    public sealed override IToken? Token { get; set; }
}