namespace ABulkCopy.Common.Tree;

public class Node
{
    public Node(TableDefinition val)
    {
        Value = val;
    }

    public string Name => Value.Header.Name;
    public TableDefinition Value { get; }

    public HashSet<Node> Children = new();
}