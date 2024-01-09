namespace ABulkCopy.APostgres;

public class PgBulkCopy : IPgBulkCopy
{
    public IDependencyGraph DependencyGraph { get; }
    private readonly ISchemaReaderFactory _schemaReaderFactory;
    private readonly ILogger _logger;



    public PgBulkCopy(
        IDependencyGraph dependencyDependencyGraph,
        ISchemaReaderFactory schemaReaderFactory,
        ILogger logger)
    {
        DependencyGraph = dependencyDependencyGraph;
        _schemaReaderFactory = schemaReaderFactory;
        _logger = logger.ForContext<PgBulkCopy>();
    }

    public async Task<string> BuildDependencyGraphAsync(
        Rdbms rdbms, List<string> schemaFiles, CancellationToken ct)
    {
        var sw = new Stopwatch();
        sw.Start();
        foreach (var schemaFile in schemaFiles)
        {
            try
            {
                var schemaReader = _schemaReaderFactory.Get(rdbms);
                DependencyGraph.Add(await schemaReader.GetTableDefinitionAsync(schemaFile, ct).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Creating TableDefinition failed with schema file '{SchemaFile}'",
                    schemaFile);
                Console.WriteLine($"Creating TableDefinition failed with schema file '{schemaFile}': {ex.FlattenMessages()}");
                throw;
            }
        }
        sw.Stop();
        var elapsed = sw.Elapsed.ToString("g");
        _logger.Information("Creating dependency graph took {Elapsed}", elapsed);
        return elapsed;
    }
}