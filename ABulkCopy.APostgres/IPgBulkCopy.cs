namespace ABulkCopy.APostgres;

public interface IPgBulkCopy
{
    Task<IEnumerable<TableDefinition>> BuildDependencyGraph(Rdbms rdbms, List<string> schemaFiles);
}