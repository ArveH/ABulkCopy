namespace ABulkCopy.Common.Graph;

public class DependencyGraph : IDependencyGraph
{
    private readonly IVisitorFactory _visitorFactory;
    private readonly Dictionary<string, Node> _children = new ();

    public DependencyGraph(IVisitorFactory visitorFactory)
    {
        _visitorFactory = visitorFactory;
    }

    public void Add(TableDefinition newTable)
    {
        // If no Foreign Keys, add to root
        if (newTable.ForeignKeys.Count == 0)
        {
            _children.Add(newTable.Header.Name, new Node(newTable));
        }

        // Check if newTable is Foreign Key for an existing table
            // Add newTable as Parent for existing table
            // Add to existing table to newTable.Children

        // Foreach Foreign Key
            // Find existing table and add it to newTable.Parents
    }

    public int Count()
    {
        var counter = _visitorFactory.GetCounterVisitor();
        foreach (var child in _children)
        {
            child.Value.Accept(counter);
        }

        return counter.Count;
    }

    public IEnumerable<TableDefinition> TablesInOrder => _children.Select(c => c.Value.Value);
}