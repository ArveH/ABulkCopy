namespace AParser.Tree.Nodes;

public class NumberNode : ConstantNode
{
    public override NodeType NodeType => NodeType.NumberNode;
    public required string Value { get; init; }

    public override T Create<T>()
    {
        throw new NotImplementedException();
    }
}