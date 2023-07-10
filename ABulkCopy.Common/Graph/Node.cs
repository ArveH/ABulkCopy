namespace ABulkCopy.Common.Graph;

public class Node
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

    public Dictionary<string, Node> Parents = new();
    public Dictionary<string, Node> Children = new();

    public void Accept(IVisitor visitor, int depth)
    {
        visitor.Visit(this, depth);

        foreach (var child in Children)
        {
            child.Value.Accept(visitor, depth+1);
        }
    }
}