namespace AParser.ParseTree.Nodes;

public class ParenthesesNode : NodeBase
{
    public override NodeType Type => NodeType.ParenthesesNode;
    public override List<INode>? Children { get; set; } = new();
}