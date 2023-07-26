namespace ABulkCopy.APostgres;

public interface IPgBulkCopy
{
    public IDependencyGraph DependencyGraph { get; }
    Task BuildDependencyGraph(Rdbms rdbms, List<string> schemaFiles);
}