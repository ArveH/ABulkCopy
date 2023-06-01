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
        var needTypeConversion = false;
        var fullPath = Path.Combine(folderPath, $"{tableName}{DbServer.Postgres.SchemaSuffix()}");
        if (!_fileSystem.File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, $"{tableName}{DbServer.SqlServer.SchemaSuffix()}");
            if (_fileSystem.File.Exists(fullPath))
            {
                _logger.Error("Schema file not found for table '{TableName}", tableName);
                throw new FileNotFoundException($"Schema file not found: {fullPath}");
            }

            needTypeConversion = true;
        }

        using var reader = _fileSystem.File.OpenText(fullPath);
        var jsonTxt = await reader.ReadToEndAsync();
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new PgColumnInterfaceConverter() }
        };  
        var sourceDefinition = JsonSerializer.Deserialize<TableDefinition>(jsonTxt, options);
        if (sourceDefinition == null)
        {
            _logger.Error("Failed to deserialize schema file for table '{TableName}", tableName);
            throw new InvalidOperationException($"Failed to deserialize schema file: {fullPath}");
        }

        if (needTypeConversion)
        {
            var schema = sourceDefinition.Header.Schema == "dbo" ? "public" : sourceDefinition.Header.Schema;
            var location = sourceDefinition.Header.Location == "PRIMARY" ? null : sourceDefinition.Header.Location;
            sourceDefinition = new TableDefinition(DbServer.Postgres)
            {
                Header = new TableHeader
                {
                    Name = sourceDefinition.Header.Name,
                    Schema = schema,
                    Location = location
                }
            };
        }

        return sourceDefinition;
    }
}