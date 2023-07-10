namespace ABulkCopy.APostgres;

public interface IPgBulkCopy
{
    Task BuildDependencyGraph(Rdbms rdbms, List<string> schemaFiles);
}