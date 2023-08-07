namespace ABulkCopy.Common.Graph.Visitors;

public class AddNodeVisitor : VisitorBase, IAddNodeVisitor
{
    private readonly INode _newNode;

    public AddNodeVisitor(INode newNode)
    {
        if (newNode.TableDefinition == null)
            throw new ArgumentNullException(
                nameof(newNode.TableDefinition),
                "You can't create AddNodeVisitor with the root node");
        _newNode = newNode;
    }

    public override void Visit(INode node, int depth)
    {
        base.Visit(node, depth);
        if (CurrentDependsOnNewNode(node))
        {
            node.Parents.TryAdd(_newNode.Name, _newNode);
            RemoveFromRootIfNeeded(node);
            _newNode.Children.TryAdd(node.Name, node);
            return;
        }

        if (NewNodeDependsOnCurrent(node))
        {
            _newNode.Parents.TryAdd(node.Name, node);
            node.Children.TryAdd(_newNode.Name, _newNode);
            IsAdded = true;
        }
    }

    // If a Child is added before it's Parent, it is temporarily added to Root,
    // so we must remove it from there
    private static void RemoveFromRootIfNeeded(INode node)
    {
        if (node.Parents.TryGetValue("root", out var root))
        {
            root.Children.Remove(node.Name);
            node.Parents.Remove("root");
        }
    }

    private bool CurrentDependsOnNewNode(INode node)
    {
        return node.TableDefinition != null &&
               node.TableDefinition.ForeignKeys.Any(fk => fk.TableReference == _newNode.Name);
    }

    bool NewNodeDependsOnCurrent(INode node)
    {
        return _newNode.TableDefinition!.ForeignKeys.Any(fk => fk.TableReference == node.Name);
    }

    public bool IsAdded { get; private set; }
}