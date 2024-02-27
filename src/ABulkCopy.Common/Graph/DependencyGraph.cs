namespace ABulkCopy.Common.Graph;

public class DependencyGraph : IDependencyGraph
{
    private readonly INodeFactory _nodeFactory;
    private readonly IVisitorFactory _visitorFactory;
    private readonly INode _root;

    public DependencyGraph(
        INodeFactory nodeFactory,
        IVisitorFactory visitorFactory)
    {
        _nodeFactory = nodeFactory;
        _visitorFactory = visitorFactory;
        _root = _nodeFactory.CreateRootNode();
    }

    public void Add(TableDefinition newTable)
    {
        var newNode = _nodeFactory.CreateNode(newTable);
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
        var allTablesWithDuplicates = BreathFirst().Where(n => n.TableDefinition != null).Select(n => n.TableDefinition!).ToArray();
        for (var i = 0; i < allTablesWithDuplicates.Length; i++)
        {
            var tabDef = allTablesWithDuplicates[i];
            if (TableExistsLater(tabDef, allTablesWithDuplicates, i))
            {
                continue;
            }
            yield return tabDef;
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

    public IEnumerable<INode> BreathFirst()
    {
        var queue = new Queue<INode>();

        foreach (var child in _root.Children)
        {
            queue.Enqueue(child.Value);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;
            foreach (var child in current.Children)
            {
                queue.Enqueue(child.Value);
            }
        }
    }
}