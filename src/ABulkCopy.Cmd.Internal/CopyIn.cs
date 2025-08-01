﻿namespace ABulkCopy.Cmd.Internal;

public class CopyIn : ICopyIn
{
    private readonly IConfiguration _config;
    private readonly IPgSystemTables _systemTables;
    private readonly IPgCmd _pgCmd;
    private readonly IPgBulkCopy _pgBulkCopy;
    private readonly IADataReaderFactory _aDataReaderFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        IConfiguration config,
        IPgSystemTables systemTables,
        IPgCmd pgCmd,
        IPgBulkCopy pgBulkCopy,
        IADataReaderFactory aDataReaderFactory,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _config = config;
        _systemTables = systemTables;
        _pgCmd = pgCmd;
        _pgBulkCopy = pgBulkCopy;
        _aDataReaderFactory = aDataReaderFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task RunAsync(Rdbms rdbms, CancellationToken ct)
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

        _logger.Information($"Creating {{TableCount}} {"table".Plural(schemaFiles.Count)}...",
            schemaFiles.Count);
        Console.WriteLine($"Creating {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}...");

        var elapsedStr = await _pgBulkCopy.BuildDependencyGraphAsync(rdbms, schemaFiles, ct).ConfigureAwait(false);
        Console.WriteLine($"Creating dependency graph took {elapsedStr}");
        var allTables = _pgBulkCopy.DependencyGraph.BreathFirst().ToList();

        var errors = 0;
        ITableSequencer tableSequencer = new TableSequencer(
            allTables.Where(t => !t.IsIndependent).DistinctBy(n => n.Name),
            allTables.Where(t => t.IsIndependent),
            _logger);

        var emptyStringFlag = _config.ToEnum(Constants.Config.EmptyString);
        var skipCreate = Convert.ToBoolean(_config[Constants.Config.SkipCreate]);
        var insertSettings = new InsertSettings
        {
            EmptyStringFlag = emptyStringFlag,
            SkipCreate = skipCreate
            // SkipZeroByteInString is always true and not configurable yet 
        };

        await Parallel.ForEachAsync(
            tableSequencer.GetTablesReadyForCreationAsync(),
            ct,
            async (node, _) =>
            {
                if (node.TableDefinition == null) throw new ArgumentNullException(nameof(node.TableDefinition));
                if (!await CreateAndInsertAsync(
                            folder,
                            node.TableDefinition,
                            insertSettings,
                            ct)
                        .ConfigureAwait(false))
                {
                    Interlocked.Increment(ref errors);
                }

                tableSequencer.TableFinished(node);
            }).ConfigureAwait(false);

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

    private async Task<bool> CreateAndInsertAsync(
        string folder,
        TableDefinition tableDefinition,
        InsertSettings insertSettings,
        CancellationToken ct)
    {
        if (!insertSettings.SkipCreate)
        {
            if (!await RecreateTableAsync(tableDefinition, ct)
                    .ConfigureAwait(false))
            {
                return false;
            }
        }

        if (!await InsertDataAsync(folder, tableDefinition, insertSettings, ct)
                .ConfigureAwait(false))
        {
            return false;
        }

        var isIdentityColumnsOk = await ResetIdentityColumnsAsync(tableDefinition, ct)
            .ConfigureAwait(false);

        var isIndexesOk = true;
        if (!insertSettings.SkipCreate)
        {
            isIndexesOk = await CreateIndexesAsync(tableDefinition, ct)
                .ConfigureAwait(false);
        }

        return isIdentityColumnsOk && isIndexesOk;
    }

    private async Task<bool> RecreateTableAsync(
        TableDefinition tableDefinition,
        CancellationToken ct)
    {
        try
        {
            await _pgCmd.DropTableAsync(tableDefinition.GetNameTuple(), ct).ConfigureAwait(false);
            await _pgCmd.CreateTableAsync(tableDefinition, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Couldn't create table '{TableName}'",
                tableDefinition.GetFullName());
            Console.WriteLine($"Couldn't create table '{tableDefinition.GetFullName()}'");
            return false;
        }

        return true;
    }

    private async Task<bool> InsertDataAsync(
        string folder,
        TableDefinition tableDefinition,
        InsertSettings insertSettings,
        CancellationToken ct)
    {
        IADataReader? dataReader = null;
        try
        {
            dataReader = _aDataReaderFactory.Get(tableDefinition.Rdbms);
            var rows = await dataReader.ReadAsync(folder, tableDefinition, ct, insertSettings).ConfigureAwait(false);
            Console.WriteLine($"Read {rows} {"row".Plural(rows)} for table '{tableDefinition.GetFullName()}'");
            _logger.Information($"Read {{Rows}} {"row".Plural(rows)} for table '{{TableName}}'",
                rows, tableDefinition.GetFullName());
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Read data for table '{TableName}' failed",
                tableDefinition.GetFullName());
            Console.WriteLine($"Read data for table '{tableDefinition.GetFullName()}' failed");
            return false;
        }
        finally
        {
            dataReader?.Dispose();
        }

        return true;
    }

    private async Task<bool> ResetIdentityColumnsAsync(
        TableDefinition tableDefinition,
        CancellationToken ct)
    {
        foreach (var columnName in tableDefinition.Columns
                     .Where(c => c.Identity != null)
                     .Select(c => c.Name))
        {
            try
            {
                await _systemTables.ResetIdentityAsync(tableDefinition.GetFullName(), columnName, ct)
                    .ConfigureAwait(false);
                _logger.Information("Reset auto generation for {TableName}.{ColumnName}",
                    tableDefinition.GetFullName(), columnName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Reset auto generation for {TableName}.{ColumnName} failed",
                    tableDefinition.GetFullName(), columnName);
                Console.WriteLine(
                    $"**ERROR**: Reset auto generation for {tableDefinition.GetFullName()}.{columnName} failed after all rows where inserted. This is a serious error! The auto generation will most likely produce duplicates.");
                return false;
            }
        }

        return true;
    }

    private async Task<bool> CreateIndexesAsync(
        TableDefinition tableDefinition,
        CancellationToken ct)
    {
        var errorOccurred = false;
        await Parallel.ForEachAsync(tableDefinition.Indexes, ct, async (indexDefinition, _) =>
        {
            try
            {
                _logger.Information("Creating index '{IndexName}' for table '{TableName}'...",
                    indexDefinition.Header.Name, tableDefinition.GetFullName());
                await _pgCmd.CreateIndexAsync(tableDefinition.GetNameTuple(), indexDefinition, ct)
                    .ConfigureAwait(false);
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.GetFullName()}'");
                _logger.Information("Created index '{IndexName}' for table '{TableName}'",
                    indexDefinition.Header.Name, tableDefinition.GetFullName());
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Created index '{indexDefinition.Header.Name}' for table '{tableDefinition.GetFullName()}' failed");
                _logger.Error(ex, "Created index '{IndexName}' for table '{TableName}' failed",
                    indexDefinition.Header.Name, tableDefinition.GetFullName());
                errorOccurred = true;
            }
        }).ConfigureAwait(false);

        return !errorOccurred;
    }
}