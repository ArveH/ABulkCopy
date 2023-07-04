namespace ABulkCopy.Cmd;

public class CopyIn : ICopyIn
{
    private readonly IPgCmd _pgCmd;
    private readonly ISchemaReaderFactory _schemaReaderFactory;
    private readonly IADataReaderFactory _aDataReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        IPgCmd pgCmd,
        ISchemaReaderFactory schemaReaderFactory,
        IADataReaderFactory aDataReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _pgCmd = pgCmd;
        _schemaReaderFactory = schemaReaderFactory;
        _aDataReaderFactory = aDataReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Run(string folder, Rdbms rdbms)
    {
        var sw = new Stopwatch();
        sw.Start();
        var schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*{Constants.SchemaSuffix}").ToList();
        _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)}",
            schemaFiles.Count);
        Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}.");
        var errors = 0;
        await Parallel.ForEachAsync(schemaFiles, async (f, _) =>
        {
            if (!await CreateTable(folder, f))
            {
                Interlocked.Increment(ref errors);
            }
        });

        if (errors > 0)
        {
            _logger.Warning(
                $"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)} finished with {{Errors}} {"error".Plural(errors)}",
                schemaFiles.Count, errors);
            Console.WriteLine(
                $"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)} finished with {errors} {"error".Plural(errors)}");
        }
        else
        {
            _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)} finished.",
                schemaFiles.Count);
            Console.WriteLine(
                $"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)} finished.");
        }
        sw.Stop();
        _logger.Information("The total CopyIn operation took {Elapsed}", sw.Elapsed.ToString("g"));
        Console.WriteLine($"The total CopyIn operation took {sw.Elapsed:g}");
    }

    private async Task<bool> CreateTable(string folder, string schemaFile)
    {
        IADataReader? dataReader = null;
        try
        {
            var schemaReader = _schemaReaderFactory.Get(Rdbms.Pg);
            var tableDefinition = await schemaReader.GetTableDefinition(schemaFile);
            await _pgCmd.DropTable(tableDefinition.Header.Name);
            await _pgCmd.CreateTable(tableDefinition).ContinueWith(async _ =>
            {
                foreach (var columnName in tableDefinition.Columns
                             .Where(c => c.Identity != null)
                             .Select(c => c.Name))
                {
                    await _pgCmd.ResetIdentity(tableDefinition.Header.Name, columnName);
                    _logger.Debug("Reset identity for '{TableName}.{ColumnName}'",
                        tableDefinition.Header.Name, columnName);
                }
            });
            dataReader = _aDataReaderFactory.Get(tableDefinition.Rdbms);
            var rows = await dataReader.Read(folder, tableDefinition);
            Console.WriteLine($"Read {rows} {"row".Plural(rows)} for table '{tableDefinition.Header.Name}'");
            _logger.Information($"Read {{Rows}} {"row".Plural(rows)} for table '{{TableName}}'",
                rows, tableDefinition.Header.Name);

            foreach (var indexDefinition in tableDefinition.Indexes)
            {
                await _pgCmd.CreateIndex(tableDefinition.Header.Name, indexDefinition);
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.Header.Name}'");
                _logger.Information("Created index '{IndexName}' for table '{TableName}'",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "CopyIn failed with schema file '{SchemaFile}'",
                schemaFile);
            Console.WriteLine($"CopyIn failed with schema file '{schemaFile}'");
            return false;
        }
        finally
        {
            dataReader?.Dispose();
        }
    }
}