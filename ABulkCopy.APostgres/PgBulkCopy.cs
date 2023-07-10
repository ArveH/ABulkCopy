using ABulkCopy.Common.Graph;
using ABulkCopy.Common.Reader;
using ABulkCopy.Common.Types;
using System.Diagnostics;

namespace ABulkCopy.APostgres;

public class PgBulkCopy : IPgBulkCopy
{
    private readonly IDependencyGraph _dependencyGraph;
    private readonly ISchemaReaderFactory _schemaReaderFactory;
    private readonly IADataReaderFactory _aDataReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public PgBulkCopy(
        IDependencyGraph dependencyGraph,
        ISchemaReaderFactory schemaReaderFactory,
        IADataReaderFactory aDataReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _dependencyGraph = dependencyGraph;
        _schemaReaderFactory = schemaReaderFactory;
        _aDataReaderFactory = aDataReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger.ForContext<PgBulkCopy>();
    }

    public async Task<IEnumerable<TableDefinition>> BuildDependencyGraph(Rdbms rdbms, List<string> schemaFiles)
    {
        var sw = new Stopwatch();
        sw.Start();
        foreach (var schemaFile in schemaFiles)
        {
            try
            {
                var schemaReader = _schemaReaderFactory.Get(rdbms);
                _dependencyGraph.Add(await schemaReader.GetTableDefinition(schemaFile));
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
        _logger.Information("Creating dependency graph took {Elapsed}", sw.Elapsed.ToString("g"));

        return _dependencyGraph.GetTablesInOrder();
    }

    public async Task CopyTables(string folder, Rdbms rdbms)
    {
    }

}