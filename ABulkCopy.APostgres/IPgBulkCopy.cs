namespace ABulkCopy.APostgres;

public interface IPgBulkCopy
{
    public IDependencyGraph DependencyGraph { get; }
    Task<string> BuildDependencyGraph(Rdbms rdbms, List<string> schemaFiles);
}