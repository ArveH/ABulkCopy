namespace AParser.ParseTree.Nodes;

public class FunctionNode : NodeBase
{
    public override NodeType Type => NodeType.FunctionNode;
    public override List<INode>? Children { get; set; } = new();
}