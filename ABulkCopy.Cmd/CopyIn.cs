using ABulkCopy.Common.Types.Index;

namespace ABulkCopy.Cmd;

public class CopyIn : ICopyIn
{
    private readonly IPgCmd _pgCmd;
    private readonly IPgBulkCopy _pgBulkCopy;
    private readonly IADataReaderFactory _aDataReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        IPgCmd pgCmd,
        IPgBulkCopy pgBulkCopy,
        IADataReaderFactory aDataReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _pgCmd = pgCmd;
        _pgBulkCopy = pgBulkCopy;
        _aDataReaderFactory = aDataReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Run(string folder, Rdbms rdbms)
    {
        var sw = new Stopwatch();
        sw.Start();

        if (!_fileSystem.Directory.Exists(folder))
        {
            _logger.Error("Folder '{Folder}' does not exist", folder);
            Console.WriteLine($"Folder '{folder}' does not exist");
            return;
        }

        var schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*{Constants.SchemaSuffix}").ToList();
        _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)}",
            schemaFiles.Count);
        Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}.");

        await _pgBulkCopy.BuildDependencyGraph(rdbms, schemaFiles);
        var allTables = _pgBulkCopy.DependencyGraph.BreathFirst().ToList();

        var errors = 0;
        ITableSequencer tableSequencer = new TableSequencer(
            allTables.Where(t => !t.IsIndependent),
            allTables.Where(t => t.IsIndependent),
            _logger);

        await Parallel.ForEachAsync(
            tableSequencer.GetTablesReadyForCreation(),
            async (node, _) =>
            {
                if (node.TableDefinition == null) throw new ArgumentNullException(nameof(node.TableDefinition));
                if (!await CreateTable(folder, node.TableDefinition))
                {
                    Interlocked.Increment(ref errors);
                }
                tableSequencer.TableFinished(node);
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

    private async Task<bool> CreateTable(string folder, TableDefinition tableDefinition)
    {
        IADataReader? dataReader = null;
        bool errorOccured = false;
        try
        {
            await _pgCmd.DropTable(tableDefinition.Header.Name);
            await _pgCmd.CreateTable(tableDefinition);

            dataReader = _aDataReaderFactory.Get(tableDefinition.Rdbms);
            var rows = await dataReader.Read(folder, tableDefinition);
            Console.WriteLine($"Read {rows} {"row".Plural(rows)} for table '{tableDefinition.Header.Name}'");
            _logger.Information($"Read {{Rows}} {"row".Plural(rows)} for table '{{TableName}}'",
                rows, tableDefinition.Header.Name);

            foreach (var columnName in tableDefinition.Columns
                         .Where(c => c.Identity != null)
                         .Select(c => c.Name))
            {
                try
                {
                    await _pgCmd.ResetIdentity(tableDefinition.Header.Name, columnName);
                    _logger.Information("Reset auto generation for {TableName}.{ColumnName}",
                        tableDefinition.Header.Name, columnName);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Reset auto generation for {TableName}.{ColumnName} failed",
                        tableDefinition.Header.Name, columnName);
                    Console.WriteLine($"**ERROR**: Reset auto generation for {tableDefinition.Header.Name}.{columnName} failed after all rows where inserted. This is a serious error! The auto generation will most likely produce duplicates.");
                    errorOccured = true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Read data for table '{TableName}' failed",
                tableDefinition.Header.Name);
            Console.WriteLine($"Read data for table '{tableDefinition.Header.Name}' failed");
            return false;
        }
        finally
        {
            dataReader?.Dispose();
        }

        await Parallel.ForEachAsync(tableDefinition.Indexes, async (indexDefinition, _) =>
        {
            try
            {
                _logger.Information("Creating index '{IndexName}' for table '{TableName}'...",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
                await _pgCmd.CreateIndex(tableDefinition.Header.Name, indexDefinition);
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.Header.Name}'");
                _logger.Information("Created index '{IndexName}' for table '{TableName}'",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.Header.Name}' failed");
                _logger.Information(ex, "Created index '{IndexName}' for table '{TableName}' failed",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
                errorOccured = true;
            }
        });

        return !errorOccured;
    }
}