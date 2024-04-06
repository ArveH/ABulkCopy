namespace ABulkCopy.Cmd.Internal.Factories;

public class SchemaReaderFactory : ISchemaReaderFactory
{
    private readonly ITypeConverter _typeConverter;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public SchemaReaderFactory(
        ITypeConverter typeConverter,
        IFileSystem fileSystem,
        ILogger logger)
    {
        _typeConverter = typeConverter;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public ISchemaReader Get(Rdbms rdbms)
    {
        if (rdbms == Rdbms.Pg)
        {
            return new PgSchemaReader(
            _typeConverter,
            _fileSystem, 
            _logger);
        }

        throw new NotSupportedException($"Rdbms {rdbms} is not supported yet");
    }
}