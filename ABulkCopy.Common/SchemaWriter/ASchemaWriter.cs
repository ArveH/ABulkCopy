namespace ABulkCopy.Common.SchemaWriter;

public class ASchemaWriter : IASchemaWriter
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public ASchemaWriter(
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
        await _fileSystem.File.WriteAllTextAsync(
            path,
            "some test json");
    }
}