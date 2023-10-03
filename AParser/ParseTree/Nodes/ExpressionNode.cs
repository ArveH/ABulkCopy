namespace AParser.ParseTree.Nodes;

public class ExpressionNode : NodeBase
{
    public override NodeType Type => NodeType.ExpressionNode;
    public override List<INode>? Children { get; set; } = new();
}