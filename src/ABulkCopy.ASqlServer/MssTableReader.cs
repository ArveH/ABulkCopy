namespace ABulkCopy.ASqlServer;

public class MssTableReader : IDisposable, IMssTableReader
{
    private readonly ISelectCreator _selectCreator;
    private readonly ILogger _logger;
    private SqlDataReader? _reader;
    private SqlCommand? _command;
    private SqlConnection? _connection;
    private long _rowCount;
    private readonly string _readerName = Guid.NewGuid().ToString();

    public MssTableReader(
        ISelectCreator selectCreator,
        ILogger logger)
    {
        _selectCreator = selectCreator;
        _logger = logger;
        _reader = null;
        _command = null;
        _connection = null;
        _rowCount = 0;
    }

    public required string ConnectionString { get; init; }
    public SqlDataReader Reader => _reader ?? throw new InvalidOperationException("Reader is not initialized");

    public async Task PrepareReaderAsync(TableDefinition tableDefinition, CancellationToken ct)
    {
        var selectStatement = _selectCreator.CreateSelect(tableDefinition);
        _connection = new SqlConnection(ConnectionString);
        _command = new SqlCommand(selectStatement);
        _command.Connection = _connection;
        await _connection.OpenAsync(ct).ConfigureAwait(false);
        _reader = await _command.ExecuteReaderAsync(
            CommandBehavior.SequentialAccess, ct).ConfigureAwait(false);
        _logger.Information("Preparing reader '{readerName}' for table '{tableName}'",
            _readerName, tableDefinition.GetFullName());
    }

    public bool IsNull(int ordinal) => Reader.IsDBNull(ordinal);
    public object GetValue(int ordinal) => Reader.GetValue(ordinal);
    public long GetBytes(int ordinal, long startIndex, byte[] buf, int length) 
        => Reader.GetBytes(ordinal, startIndex, buf, 0, length);

    public async Task<bool> ReadAsync(CancellationToken ct)
    {
        if (_reader == null)
        {
            throw new InvalidOperationException("You must call PrepareReader before Reading");
        }

        if (await _reader.ReadAsync(ct).ConfigureAwait(false))
        {
            _rowCount++;
            return true;
        }
        return false;
    }

    public void Close()
    {
        if (_reader != null)
        {
            _reader.Dispose();
            _reader = null;
            _logger.Information("Reader '{readerName}' read {rowCount} rows", 
                _readerName, _rowCount);
        }
        if (_command != null)
        {
            _command.Dispose();
            _command = null;
        }
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }
        _rowCount = 0;
    }

    public void Dispose()
    {
        Close();
    }
}