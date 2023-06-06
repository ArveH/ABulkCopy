namespace ABulkCopy.Common.Reader;

public class DataFileReaderFactory : IDataFileReaderFactory
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public DataFileReaderFactory(
        IFileSystem fileSystem,
        ILogger logger)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public IDataFileReader Create(string folder, TableDefinition tableDefinition)
    {
        var path = Path.Combine(
            folder, 
            $"{tableDefinition.Header.Name}{tableDefinition.DbServer.DataSuffix()}");
        return new DataFileReader(path, _fileSystem, tableDefinition.Columns, _logger);
    }
}