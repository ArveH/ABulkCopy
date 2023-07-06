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

    public IEnumerable<TableDefinition> GetTablesInOrder()
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