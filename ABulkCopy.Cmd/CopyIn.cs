namespace ABulkCopy.Cmd;

public class CopyIn : ICopyIn
{
    private readonly ISchemaReader _schemaReader;
    private readonly IADataReader _aDataReader;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CopyIn(
        ISchemaReader schemaReader,
        IADataReader aDataReader,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _schemaReader = schemaReader;
        _aDataReader = aDataReader;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Run(string folder, DbServer dbServer)
    {
        var schemaFiles = _fileSystem.Directory.GetFiles(folder, $"*.{dbServer.SchemaSuffix()}").ToList();
        _logger.Information($"Creating and filling {{TableCount}} {"table".Plural(schemaFiles.Count)}",
            schemaFiles.Count);
        Console.WriteLine($"Creating and filling {schemaFiles.Count} {"table".Plural(schemaFiles.Count)}.");
        var errors = 0;
        foreach (var schemaFile in schemaFiles)
        {
            await Task.CompletedTask;
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