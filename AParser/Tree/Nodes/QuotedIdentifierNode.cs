namespace AParser.Tree.Nodes;

public class QuotedIdentifierNode : NameNode
{
    public override NodeType NodeType => NodeType.QuotedIdentifierNode;
    public required string Name { get; init; }

    public override T Create<T>()
    {
        throw new NotImplementedException();
    }
}