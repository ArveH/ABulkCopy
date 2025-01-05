namespace ABulkCopy.APostgres.DbRaw;

public class PgRawReader : IPgRawReader
{
    private readonly NpgsqlDataReader _reader;
    
    internal PgRawReader(DbDataReader reader)
    {
        _reader = (NpgsqlDataReader)reader;
    }
    
    public Task<bool> ReadAsync(CancellationToken cancellationToken) => _reader.ReadAsync(cancellationToken);

    public string GetString(int ordinal) => _reader.GetString(ordinal);
    
    public short GetInt16(int ordinal) => _reader.GetInt16(ordinal);

    public int GetInt32(int ordinal) => _reader.GetInt32(ordinal);

    public decimal GetDecimal(int ordinal) => _reader.GetDecimal(ordinal);
    
    public bool GetBoolean(int ordinal) => _reader.GetBoolean(ordinal);

    public byte GetByte(int ordinal) => _reader.GetByte(ordinal);

    public bool IsDBNull(int ordinal) => _reader.IsDBNull(ordinal);

    public void Dispose()
    {
        _reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _reader.DisposeAsync();
    }
}