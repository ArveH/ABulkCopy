namespace ASqlServer;

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

    public async Task PrepareReader(TableDefinition tableDefinition)
    {
        var selectStatement = _selectCreator.CreateSelect(tableDefinition);
        _connection = new SqlConnection(ConnectionString);
        _command = new SqlCommand(selectStatement);
        _command.Connection = _connection;
        await _connection.OpenAsync().ConfigureAwait(false);
        _reader = await _command.ExecuteReaderAsync(
            CommandBehavior.SequentialAccess).ConfigureAwait(false);
        _logger.Information("Preparing reader '{readerName}' for table '{tableName}'",
            _readerName, tableDefinition.Header.Name);
    }

    public bool IsNull(int ordinal) => _reader?.IsDBNull(ordinal) ?? true;
    public object? GetValue(int ordinal) => _reader?.GetValue(ordinal);

    public async Task<bool> Read()
    {
        if (_reader == null)
        {
            throw new InvalidOperationException("You must call PrepareReader before Reading");
        }

        if (await _reader.ReadAsync().ConfigureAwait(false))
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