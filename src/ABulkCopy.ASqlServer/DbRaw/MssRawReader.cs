namespace ABulkCopy.ASqlServer.DbRaw;

public class MssRawReader : IMssRawReader
{
    private readonly SqlDataReader _reader;

    internal MssRawReader(SqlDataReader reader)
    {
        _reader = reader;
    }
    
    public Task<bool> ReadAsync(CancellationToken cancellationToken) => _reader.ReadAsync(cancellationToken);

    public string GetString(int ordinal) => _reader.GetString(ordinal);
    
    public int GetInt32(int ordinal) => _reader.GetInt32(ordinal);
    
    public decimal GetDecimal(int ordinal) => _reader.GetDecimal(ordinal);
    
    public bool IsDBNull(int ordinal) => _reader.IsDBNull(ordinal);

    public void Dispose()
    {
        _reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _reader.DisposeAsync().ConfigureAwait(false);
    }
}