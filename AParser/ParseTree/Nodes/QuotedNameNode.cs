namespace AParser.ParseTree.Nodes;

public class QuotedNameNode : NodeBase
{
    public override NodeType Type => NodeType.QuotedNameNode;
    public override List<INode>? Children { get; set; } = new();
}