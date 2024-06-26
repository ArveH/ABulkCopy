﻿namespace ABulkCopy.Common.Writer;

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

    public async Task WriteAsync(
        TableDefinition tableDefinition,
        string path)
    {
        _logger.Debug("Writing table definition for '{TableName}' to file...",
            tableDefinition.GetFullName());
        var fullPath = Path.Combine(path, tableDefinition.GetSchemaFileName());
        await _fileSystem.File.WriteAllTextAsync(
            fullPath,
            JsonSerializer.Serialize(tableDefinition, new JsonSerializerOptions
            {
                // Contrast is going to have a field day with me allowing stuff like ' :-)
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
            })).ConfigureAwait(false);
        _logger.Information("Table definition written to '{FullPath}'",
            fullPath);
    }
}