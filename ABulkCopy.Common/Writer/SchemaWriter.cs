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
        // JsonSerializer doesn't handle interfaces, so we need to use anonymous objects
        _logger.Debug("Serializing table definition for '{TableName}'...", 
            tableDefinition.Header.Name);
        object columns = tableDefinition.Columns.Select(c => new
        {
            c.Name,
            // We want the Type property to be the name of the enum value
            // (not the value itself)
            Type = c.Type.ToString(),
            c.IsNullable,
            c.Identity,
            c.ComputedDefinition,
            c.Length,
            c.Precision,
            c.Scale,
            c.DefaultConstraint,
            c.Collation
        }).ToList();
        var json = new Dictionary<string, object?>()
        {
            { "Header", tableDefinition.Header },
            { "Columns", columns },
            { "PrimaryKey", tableDefinition.PrimaryKey },
            { "ForeignKeys", tableDefinition.ForeignKeys }
        };

        _logger.Debug("Writing table definition for '{TableName}' to file...", 
            tableDefinition.Header.Name);
        var fullPath = Path.Combine(path, tableDefinition.Header.Name + CommonConstants.SchemaSuffix);
        await _fileSystem.File.WriteAllTextAsync(
            fullPath,
            JsonSerializer.Serialize(json, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        _logger.Information("Table definition written to '{FullPath}'", 
            fullPath);
    }
}