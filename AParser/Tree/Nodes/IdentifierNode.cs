namespace AParser.Tree.Nodes;

public class IdentifierNode : NameNode
{
    public override NodeType NodeType => NodeType.IdentifierNode;
    public required string Name { get; init; }

    public override T Create<T>()
    {
        throw new NotImplementedException();
    }
}