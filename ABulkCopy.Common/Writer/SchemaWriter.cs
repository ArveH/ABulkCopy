namespace ABulkCopy.Common.Writer;

public class SchemaWriter : ISchemaWriter
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public SchemaWriter(
        IFileSystem fileSystem,
        ILogger logger)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Write(
        TableDefinition tableDefinition,
        string path)
    {
        _logger.Debug("Writing table definition for '{TableName}' to file...",
            tableDefinition.Header.Name);
        var fullPath = Path.Combine(path, tableDefinition.Header.Name + tableDefinition.DbServer.SchemaSuffix());
        await _fileSystem.File.WriteAllTextAsync(
            fullPath,
            JsonSerializer.Serialize(tableDefinition, new JsonSerializerOptions
            {
                // Contrast is going to have a field day with me allowing stuff like ' :-)
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
            }));
        _logger.Information("Table definition written to '{FullPath}'",
            fullPath);
    }
}