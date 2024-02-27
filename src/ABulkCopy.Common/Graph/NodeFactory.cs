namespace ABulkCopy.Common.Graph;

public class NodeFactory : INodeFactory
{
    public INode CreateRootNode()
    {
        return new Node();
    }

    public INode CreateNode(TableDefinition tableDefinition)
    {
        return new Node(tableDefinition);
    }
}