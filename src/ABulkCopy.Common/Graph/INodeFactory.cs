namespace ABulkCopy.Common.Graph;

public interface INodeFactory
{
    INode CreateRootNode();
    INode CreateNode(TableDefinition tableDefinition);
}