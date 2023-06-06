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

    public IDataFileReader Create(string path, IReadOnlyList<IColumn> columns)
    {
        return new DataFileReader(path, _fileSystem, columns, _logger);
    }
}