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
        Read();
        RowCounter = 0;
    }

    public long RowCounter { get; private set; }
    public int CurrentChar { get; private set; }

    public string? ReadColumn(string colName)
    {
        _logger.Verbose("Reading value for row {RowCount} column '{ColumnName}'",
            RowCounter, colName);
        _columnHolder.Clear();
        while (CurrentChar >= 0 && CurrentChar != ColumnSeparator)
        {
            _columnHolder.Append((char)CurrentChar);
            Read();
        }
        ReadColumnSeparator(colName);

        return _columnHolder.Length == 0 ? null : _columnHolder.ToString();
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

        Read();
    }

    public void ReadNewLine()
    {
        if (CurrentChar != '\n' && CurrentChar != '\r' && !IsEndOfFile)
        {
            _logger.Error("Newline not found in correct position in line {RowCount}. " +
                          "Found '{CurrentChar}'",
                RowCounter, CurrentChar);
            throw new NotValidDataException(
                $"Newline not found in correct position in line {RowCounter}. " +
                $"Found '{CurrentChar}'");
        }
        Read();
        if (CurrentChar == '\n')
        {
            Read();
        }

        RowCounter++;
    }

    private void Read()
    {
        CurrentChar = _stream.Read();
    }

    public bool IsEndOfFile => CurrentChar == -1;

    public void Dispose()
    {
        _stream.Dispose();
    }
}