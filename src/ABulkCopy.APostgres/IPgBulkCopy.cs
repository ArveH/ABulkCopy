namespace ABulkCopy.APostgres;

public interface IPgBulkCopy
{
    public IDependencyGraph DependencyGraph { get; }
    Task<string> BuildDependencyGraphAsync(Rdbms rdbms, List<string> schemaFiles, CancellationToken ct);
}