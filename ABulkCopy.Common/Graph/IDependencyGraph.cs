namespace ABulkCopy.Common.Graph;

public interface IDependencyGraph
{
    public void Add(TableDefinition newTable);
    public int Count();
    public IEnumerable<TableDefinition> GetTablesInOrder();
    List<TableDepth> GetTableDepths();
    IEnumerable<Node> BreathFirst();
}