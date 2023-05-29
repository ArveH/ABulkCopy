namespace ABulkCopy.APostgres.Reader;

public class PgSchemaReader : ISchemaReader
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public PgSchemaReader(
        IFileSystem fileSystem,
        ILogger logger)
    {
        _fileSystem = fileSystem;
        _logger = logger.ForContext<PgSchemaReader>();
    }

    public async Task<TableDefinition?> GetTableDefinition(string folderPath, string tableName)
    {
        var fullPath = Path.Combine(folderPath, $"{tableName}{CommonConstants.SchemaSuffix}");
        if (!_fileSystem.File.Exists(fullPath))
        {
            _logger.Error("Schema file not found: {FullPath}", fullPath);
            throw new FileNotFoundException($"Schema file not found: {fullPath}");
        }

        using var reader = _fileSystem.File.OpenText(fullPath);
        var json = await reader.ReadToEndAsync();
        var tableDefinition = JsonSerializer.Deserialize<TableDefinition>(json);

        return tableDefinition;
    }
}