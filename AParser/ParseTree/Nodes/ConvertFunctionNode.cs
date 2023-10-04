namespace AParser.ParseTree.Nodes;

public class ConvertFunctionNode : NodeBase
{
    public override NodeType Type => NodeType.ConvertFunctionNode;
    public override List<INode>? Children { get; set; } = new();
}