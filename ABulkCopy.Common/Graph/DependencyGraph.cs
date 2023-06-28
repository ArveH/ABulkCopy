namespace ABulkCopy.Common.Graph;

public class DependencyGraph
{
    private readonly Dictionary<string, Node> _children = new ();

    public DependencyGraph(IEnumerable<TableDefinition> tableDefinitions)
    {
        foreach (var tableDefinition in tableDefinitions)
        {
            Add(tableDefinition);
        }
    }

    private void Add(TableDefinition tableDefinition)
    {
        _children.Add(tableDefinition.Header.Name, new Node(tableDefinition));
    }

    public int Count => _children.Count;
    public IEnumerable<TableDefinition> TablesInOrder => _children.Select(c => c.Value.Value);
}