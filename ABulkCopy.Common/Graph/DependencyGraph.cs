namespace ABulkCopy.Common.Graph;

public class DependencyGraph : IDependencyGraph
{
    private readonly IVisitorFactory _visitorFactory;
    private readonly Node _root = new();

    public DependencyGraph(IVisitorFactory visitorFactory)
    {
        _visitorFactory = visitorFactory;
    }

    public void Add(TableDefinition newTable)
    {
        var newNode = new Node(newTable);
        var addNodeVisitor = _visitorFactory.GetAddNodeVisitor(newNode);

        _root.Accept(addNodeVisitor, 0);

        if (addNodeVisitor.IsAdded) return;

        newNode.Parents.Add(_root.Name, _root);
        _root.Children.Add(newNode.Name, newNode);
    }

    public int Count()
    {
        var counterVisitor = _visitorFactory.GetCounterVisitor();
        _root.Accept(counterVisitor, 0);

        return counterVisitor.NodeCount; // we don't count root node
    }

    public List<TableDepth> GetTableDepths()
    {
        var depthVisitor = _visitorFactory.GetDepthVisitor();
        _root.Accept(depthVisitor, 0);

        return depthVisitor.Result;
    }

    // TODO: Re-think how to get tables in order
    // This will get the tables in the correct order, but it's not very efficient.
    // I also can't use it since I need to know that a parent table has completed
    // before I can start coping the child table.
    // I need to rethink the whole process
    public IEnumerable<TableDefinition> GetTablesInOrder()
    {
        var allTablesWithDuplicates = BreathFirst().ToArray();
        for (var i = 0; i < allTablesWithDuplicates.Length; i++)
        {
            var table = allTablesWithDuplicates[i];
            if (TableExistsLater(table, allTablesWithDuplicates, i))
            {
                continue;
            }
            yield return table;
        }
    }

    private bool TableExistsLater(TableDefinition table, TableDefinition[] allTablesWithDuplicates, int currentPos)
    {
        for (var i = currentPos + 1; i < allTablesWithDuplicates.Length; i++)
        {
            if (allTablesWithDuplicates[i].Header.Name == table.Header.Name)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<TableDefinition> BreathFirst()
    {
        var queue = new Queue<Node>();

        foreach (var child in _root.Children)
        {
            queue.Enqueue(child.Value);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current.Value!;
            foreach (var child in current.Children)
            {
                queue.Enqueue(child.Value);
            }
        }
    }
}