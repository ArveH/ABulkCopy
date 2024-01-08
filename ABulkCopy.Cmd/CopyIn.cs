namespace ABulkCopy.Cmd;

public class CopyIn : ICopyIn
{
    private readonly IConfiguration _config;
    private readonly IPgCmd _pgCmd;
    private readonly IPgBulkCopy _pgBulkCopy;
    private readonly IADataReaderFactory _aDataReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        IConfiguration config,
        IPgCmd pgCmd,
        IPgBulkCopy pgBulkCopy,
        IADataReaderFactory aDataReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _config = config;
        _pgCmd = pgCmd;
        _pgBulkCopy = pgBulkCopy;
        _aDataReaderFactory = aDataReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task RunAsync(Rdbms rdbms)
    {
        var sw = new Stopwatch();
        sw.Start();

        var folder = _config.Check(Constants.Config.Folder);
        if (!_fileSystem.Directory.Exists(folder))
        {
            _logger.Error("Folder '{Folder}' does not exist", folder);
            Console.WriteLine($"Folder '{folder}' does not exist");
            return;
        }

        List<string>? schemaFiles;
        var searchFilter = _config[Constants.Config.SearchFilter];
        if (!string.IsNullOrWhiteSpace(searchFilter))
        {
            schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*{Constants.SchemaSuffix}").AsEnumerable()
                .Where(f => Regex.IsMatch(f, searchFilter))
                .ToList();
        }
        else
        {
            schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*{Constants.SchemaSuffix}").ToList();
        }

        _logger.Information($"Creating {{TableCount}} {"table".Plural(schemaFiles.Count)}",
            schemaFiles.Count);
        Console.WriteLine($"Creating {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}.");

        var elapsedStr = await _pgBulkCopy.BuildDependencyGraphAsync(rdbms, schemaFiles);
        Console.WriteLine($"Creating dependency graph took {elapsedStr}");
        var allTables = _pgBulkCopy.DependencyGraph.BreathFirst().ToList();

        var errors = 0;
        ITableSequencer tableSequencer = new TableSequencer(
            allTables.Where(t => !t.IsIndependent).DistinctBy(n => n.Name), 
            allTables.Where(t => t.IsIndependent),
            _logger);

        await Parallel.ForEachAsync(
            tableSequencer.GetTablesReadyForCreationAsync(),
            async (node, _) =>
            {
                if (node.TableDefinition == null) throw new ArgumentNullException(nameof(node.TableDefinition));
                if (!await CreateTableAsync(folder, node.TableDefinition, _config.ToEnum(Constants.Config.EmptyString)))
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

    private async Task<bool> CreateTableAsync(
        string folder, 
        TableDefinition tableDefinition,
        EmptyStringFlag emptyStringFlag)
    {
        IADataReader? dataReader = null;
        var errorOccurred = false;
        try
        {
            try
            {
                await _pgCmd.DropTableAsync(tableDefinition.Header.Name);
                await _pgCmd.CreateTableAsync(tableDefinition);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Couldn't create table '{TableName}'",
                    tableDefinition.Header.Name);
                Console.WriteLine($"Couldn't create table '{tableDefinition.Header.Name}'");
                return false;
            }

            try
            {
                dataReader = _aDataReaderFactory.Get(tableDefinition.Rdbms);
                var rows = await dataReader.ReadAsync(folder, tableDefinition, emptyStringFlag);
                Console.WriteLine($"Read {rows} {"row".Plural(rows)} for table '{tableDefinition.Header.Name}'");
                _logger.Information($"Read {{Rows}} {"row".Plural(rows)} for table '{{TableName}}'",
                    rows, tableDefinition.Header.Name);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Read data for table '{TableName}' failed",
                    tableDefinition.Header.Name);
                Console.WriteLine($"Read data for table '{tableDefinition.Header.Name}' failed");
                return false;
            }

            foreach (var columnName in tableDefinition.Columns
                         .Where(c => c.Identity != null)
                         .Select(c => c.Name))
            {
                try
                {
                    await _pgCmd.ResetIdentityAsync(tableDefinition.Header.Name, columnName);
                    _logger.Information("Reset auto generation for {TableName}.{ColumnName}",
                        tableDefinition.Header.Name, columnName);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Reset auto generation for {TableName}.{ColumnName} failed",
                        tableDefinition.Header.Name, columnName);
                    Console.WriteLine($"**ERROR**: Reset auto generation for {tableDefinition.Header.Name}.{columnName} failed after all rows where inserted. This is a serious error! The auto generation will most likely produce duplicates.");
                    errorOccurred = true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error for table '{TableName}'",
                tableDefinition.Header.Name);
            Console.WriteLine($"Unexpected error for table '{tableDefinition.Header.Name}'");
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
                await _pgCmd.CreateIndexAsync(tableDefinition.Header.Name, indexDefinition);
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.Header.Name}'");
                _logger.Information("Created index '{IndexName}' for table '{TableName}'",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.Header.Name}' failed");
                _logger.Error(ex, "Created index '{IndexName}' for table '{TableName}' failed",
                    indexDefinition.Header.Name, tableDefinition.Header.Name);
                errorOccurred = true;
            }
        });

        return !errorOccurred;
    }
}