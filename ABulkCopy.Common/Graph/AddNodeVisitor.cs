namespace ABulkCopy.Common.Graph;

public class AddNodeVisitor : IAddNodeVisitor
{
    private readonly Node _newNode;

    public AddNodeVisitor(Node newNode)
    {
        _newNode = newNode;
    }

    public void Visit(Node node)
    {
        if (node.Value.ForeignKeys.Any(fk => fk.TableReference == _newNode.Name))
        {
            node.Parents.TryAdd(_newNode.Name, _newNode);
            _newNode.Children.TryAdd(node.Name, node);
            return;
        }

        if (_newNode.Value.ForeignKeys.Any(fk => fk.TableReference == node.Name))
        {
            _newNode.Parents.TryAdd(node.Name, node);
            node.Children.TryAdd(_newNode.Name, _newNode);
        }
    }
}