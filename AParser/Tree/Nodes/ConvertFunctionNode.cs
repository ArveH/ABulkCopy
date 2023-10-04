namespace AParser.Tree.Nodes;

public class ConvertFunctionNode : FunctionNode
{
    public override NodeType NodeType => NodeType.ConvertFunctionNode;
    public required string Type { get; init; }
    public required string Expression { get; init; }

    public override T Create<T>()
    {
        throw new NotImplementedException();
    }
}