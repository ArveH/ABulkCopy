namespace ABulkCopy.Common.Reader;

public class DataFileReader : IDataFileReader, IDisposable
{
    private readonly IReadOnlyList<IColumn> _columns;
    private readonly ILogger _logger;
    private readonly StreamReader _stream;

    private const int QuoteChar = '"';
    private const int ColumnSeparator = ',';
    private readonly StringBuilder _columnHolder = new(10_485_760);

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

    public string ReadColumn(string colName)
    {
        _logger.Verbose("Reading value for row {RowCount} column '{ColumnName}'",
            RowCounter, colName);
        _columnHolder.Clear();
        while (CurrentChar >= 0 && CurrentChar != ColumnSeparator)
        {
            CurrentChar = _stream.Read();
            _columnHolder.Append(CurrentChar);
        }

        return _columnHolder.ToString();
    }

    public void ReadColumnSeparator(string colName)
    {
        if (CurrentChar != ',')
        {
            _logger.Error("Data for column '{ColName}' missing field terminator " +
                          "in line {RowCounter}. Found '{CurrentChar}'",
                colName, RowCounter, CurrentChar);
            throw new NotValidDataException(
                $"Data for column '{colName}' missing field terminator " +
                $"in line {RowCounter}. Found '{CurrentChar}'");
        }

        CurrentChar = _stream.Read();
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}