namespace ABulkCopy.Common.Reader;

public class DataFileReader : IDataFileReader, IDisposable
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;
    private StreamReader? _stream;
    private StreamReader InternalStream
    {
        get => _stream ?? throw new InvalidOperationException();
        set => _stream = value;
    }

    private readonly StringBuilder _columnHolder = new(10_485_760);

    public DataFileReader(
        IFileSystem fileSystem,
        ILogger logger)
    {
        _fileSystem = fileSystem;
        _stream = null;
        _logger = logger.ForContext<DataFileReader>();
    }

    public long RowCounter { get; private set; }
    public int CurrentChar { get; private set; }

    public void Open(string path)
    {
        var fileStream = _fileSystem.FileStream.New(path, FileMode.Open);
        InternalStream = new StreamReader(fileStream, new UTF8Encoding(false));
        ReadChar();
        RowCounter = 0;
    }

    public byte[] ReadAllBytes(string path)
    {
        return _fileSystem.File.ReadAllBytes(path);
    }

    public string? ReadColumn(string colName)
    {
        _logger.Verbose("Reading value for column '{ColumnName}' row {RowCount}",
            RowCounter, colName);
        _columnHolder.Clear();
        if (CurrentChar == Constants.QuoteChar)
        {
            ReadQuotedValue(colName);
        }
        else
        {
            ReadUnquotedValue();
        }
        ReadColumnSeparator(colName);

        return _columnHolder.Length == 0 ? null : _columnHolder.ToString();
    }

    private void ReadQuotedValue(string colName)
    {
        ReadQuote(colName, "opening");
        while (CurrentChar >= 0)
        {
            if (CurrentChar == Constants.QuoteChar)
            {
                if (InternalStream.Peek() == Constants.QuoteChar)
                {
                    ReadChar();
                }
                else
                {
                    break;
                }
            }
            AddChar();
            ReadChar();
        }
        ReadQuote(colName, "closing");
    }

    private void ReadUnquotedValue()
    {
        while (CurrentChar >= 0 && CurrentChar != Constants.ColumnSeparatorChar)
        {
            AddChar();
            ReadChar();
        }
    }

    private void AddChar()
    {
        _columnHolder.Append((char)CurrentChar);
    }

    // quotePlacement is either "opening" or "closing"
    public void ReadQuote(string colName, string quotePlacement)
    {
        if (CurrentChar != Constants.QuoteChar)
        {
            _logger.Error($"Expected {quotePlacement} quote for column '{{ColName}}' " +
                          "in line {RowCounter}. Found '{CurrentChar}'",
                colName, RowCounter, CurrentChar);
            throw new NotValidDataException(
                $"Expected {quotePlacement} quote for column '{colName}' " +
                $"in line {RowCounter}. Found '{CurrentChar}'");
        }

        ReadChar();
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

        ReadChar();
    }

    public void SkipToNextLine()
    {
        while (CurrentChar != '\n' && CurrentChar != '\r' && !IsEndOfFile)
        {
            ReadChar();
        }
        ReadNewLine();
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
        ReadChar();
        if (CurrentChar == '\n')
        {
            ReadChar();
        }

        RowCounter++;
    }

    private void ReadChar()
    {
        CurrentChar = InternalStream.Read();
    }

    public bool IsEndOfFile => CurrentChar == -1;

    public void Dispose()
    {
        _stream?.Dispose();
    }
}