﻿namespace ABulkCopy.APostgres.Reader;

public class PgSchemaReader : ISchemaReader
{
    private readonly ITypeConverter _typeConverter;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public PgSchemaReader(
        ITypeConverter typeConverter,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _typeConverter = typeConverter;
        _fileSystem = fileSystem;
        _logger = logger.ForContext<PgSchemaReader>();
    }

    public async Task<TableDefinition> GetTableDefinitionAsync(
        string folderPath, SchemaTableTuple st, CancellationToken ct)
    {
        var fullPath = Path.Combine(folderPath, st.GetSchemaFileName());
        return await GetTableDefinitionAsync(fullPath, ct).ConfigureAwait(false);
    }

    public async Task<TableDefinition> GetTableDefinitionAsync(string fullPath, CancellationToken ct)
    {
        if (!_fileSystem.File.Exists(fullPath))
        {
            _logger.Error("Schema file not found: '{FullPath}'", fullPath);
            throw new FileNotFoundException($"Schema file not found: '{fullPath}'");
        }

        using var reader = _fileSystem.File.OpenText(fullPath);
        var jsonTxt = await reader.ReadToEndAsync(ct).ConfigureAwait(false);
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new PgColumnInterfaceConverter() }
        };
        var tableDefinition = JsonSerializer.Deserialize<TableDefinition>(jsonTxt, options);
        if (tableDefinition == null)
        {
            _logger.Error("Failed to deserialize schema file '{SchemaFile}", fullPath);
            throw new InvalidOperationException($"Failed to deserialize schema file: {fullPath}");
        }

        if (tableDefinition.Rdbms != Rdbms.Pg)
        {
            tableDefinition = _typeConverter.Convert(tableDefinition);
        }

        return tableDefinition;
    }
}