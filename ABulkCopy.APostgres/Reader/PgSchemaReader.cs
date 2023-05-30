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
        var jsonTxt = await reader.ReadToEndAsync();
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };  
        var tableDefinition = JsonSerializer.Deserialize<TableDefinition>(jsonTxt, options);

        return tableDefinition;
    }
}