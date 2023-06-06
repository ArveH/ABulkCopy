namespace ABulkCopy.Common.Reader;

public class DataFileReader : IDataFileReader, IDisposable
{
    private readonly IReadOnlyList<IColumn> _columns;
    private readonly ILogger _logger;
    private readonly StreamReader _stream;

    private const int QuoteChar = '"';
    private const int ColumnSeparator = ',';


    public DataFileReader(
        string path, 
        IFileSystem fileSystem,
        IReadOnlyList<IColumn> columns,
        ILogger logger)
    {
        _columns = columns;
        _logger = logger.ForContext<DataFileReader>();
        var fileStream = fileSystem.FileStream.New(path, FileMode.Open);
        _stream = new StreamReader(fileStream, new UTF8Encoding(false));
        CurrentChar = _stream.Read();
        RowCounter = 0;
    }

    public long RowCounter { get; }
    public int CurrentChar { get; private set; }

    public IEnumerable<char> ReadColumn(IColumn column)
    {
        _logger.Verbose("Reading value for row {RowCount} column '{ColumnName}'",
            RowCounter, column.Name);
        while (CurrentChar >= 0 && CurrentChar != ColumnSeparator)
        {
            yield return (char)CurrentChar;
            CurrentChar = _stream.Read();
        }
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}