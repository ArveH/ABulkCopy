namespace ABulkCopy.Common.Graph;

public class Node : INode
{
    public Node()
    {
        Name = "root";
    }
    public Node(TableDefinition val)
    {
        Value = val;
        Name = val.Header.Name;
    }

    public string Name { get; }

    public TableDefinition? Value { get; }

    public bool IsRoot => Name == "root";
    public bool IsIndependent => (Parents.Count == 1 && Parents.First().Key == "root") || Parents.Count == 0;

    public Dictionary<string, INode> Parents { get; } = new();
    public Dictionary<string, INode> Children { get; } = new();

    public void Accept(IVisitor visitor, int depth)
    {
        visitor.Visit(this, depth);

        foreach (var child in Children)
        {
            child.Value.Accept(visitor, depth+1);
        }
    }
}