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
        var newNode = new Node(newTable);
        var addNodeVisitor = _visitorFactory.GetAddNodeVisitor(newNode);

        if (_children.Count == 0)
        {
            _children.Add(newNode.Name, newNode);
            return;
        }

        foreach (var child in _children)
        {
            child.Value.Accept(addNodeVisitor);
        }

        if (newNode.Value.ForeignKeys.Count == 0)
        {
            _children.Add(newNode.Name, newNode);
        }

        // If no Foreign Keys, add to root

        // Check if newTable is Foreign Key for an existing table
        // Add newTable as Parent for existing table
        // Add existing table to newTable.Children

        // Foreach Foreign Key
        // Find existing table and add it to newTable.Parents
    }

    public int Count()
    {
        var counterVisitor = _visitorFactory.GetCounterVisitor();
        foreach (var child in _children)
        {
            child.Value.Accept(counterVisitor);
        }

        return counterVisitor.NodeCount;
    }

    public override string ToString()
    {
        var toStringVisitor = _visitorFactory.GetToStringVisitor();
        foreach (var child in _children)
        {
            child.Value.Accept(toStringVisitor);
        }

        return toStringVisitor.Result;
    }

    public IEnumerable<TableDefinition> TablesInOrder => _children.Select(c => c.Value.Value);
}