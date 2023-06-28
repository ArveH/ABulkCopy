namespace ABulkCopy.Common.Graph;

public interface IDependencyGraph
{
    void Add(TableDefinition newTable);
    int Count();
    IEnumerable<TableDefinition> TablesInOrder { get; }
}