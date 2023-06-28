namespace ABulkCopy.Common.Graph;

public class Node
{
    public Node(TableDefinition val)
    {
        Value = val;
    }

    public string Name => Value.Header.Name;
    public TableDefinition Value { get; }
    
    public bool HasParents => Parents.Count > 0;
    public bool HasChildren => Children.Count > 0;

    public Dictionary<string, Node> Parents = new();
    public Dictionary<string, Node> Children = new();

    public void Accept(IVisitor visitor)
    {
        visitor.Visit();
        foreach (var child in Children)
        {
            child.Value.Accept(visitor);
        }
    }
}