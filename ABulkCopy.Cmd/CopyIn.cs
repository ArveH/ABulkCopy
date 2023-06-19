namespace ABulkCopy.Cmd;

public class CopyIn : ICopyIn
{
    private readonly IPgCmd _pgCmd;
    private readonly ISchemaReader _schemaReader;
    private readonly IADataReader _aDataReader;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        IPgCmd pgCmd,
        ISchemaReader schemaReader,
        IADataReader aDataReader,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _pgCmd = pgCmd;
        _schemaReader = schemaReader;
        _aDataReader = aDataReader;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Run(string folder, Rdbms rdbms)
    {
        var schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*{Constants.SchemaSuffix}").ToList();
        _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)}",
            schemaFiles.Count);
        Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}.");
        var errors = 0;
        foreach (var schemaFile in schemaFiles)
        {
            try
            {
                var tableDefinition = await _schemaReader.GetTableDefinition(schemaFile);
                await _pgCmd.DropTable(tableDefinition.Header.Name);
                await _pgCmd.CreateTable(tableDefinition);
                var rows = await _aDataReader.Read(folder, tableDefinition);
                Console.WriteLine($"Read {rows} {"row".Plural(rows)} for table '{tableDefinition.Header.Name}'");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed with schema file '{SchemaFile}'",
                    schemaFile);
                Console.WriteLine($"Failed with schema file '{schemaFile}'");
                errors++;
            }
        }

        if (errors > 0)
        {
            _logger.Warning($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)} finished with {{Errors}} {"error".Plural(errors)}", 
                schemaFiles.Count, errors);
            Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)} finished with {errors} {"error".Plural(errors)}");
        }
        else
        {
            _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)} finished.", 
                schemaFiles.Count);
            Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)} finished.");
        }
    }
}