namespace ABulkCopy.ASqlServer.DbRaw;

public class MssRawReader : IMssRawReader
{
    private readonly SqlDataReader _reader;

    internal MssRawReader(SqlDataReader reader)
    {
        _reader = reader;
    }
    
    public Task<bool> ReadAsync(CancellationToken cancellationToken) => _reader.ReadAsync(cancellationToken);

    public char GetChar(int ordinal) => _reader.GetChar(ordinal);
    
    public string GetString(int ordinal) => _reader.GetString(ordinal);
    
    public short GetInt16(int ordinal) => _reader.GetInt16(ordinal);

    public int GetInt32(int ordinal) => _reader.GetInt32(ordinal);
    
    public long GetInt64(int ordinal) => _reader.GetInt64(ordinal);
    
    public uint GetUInt32(int ordinal) => _reader.GetFieldValue<uint>(ordinal);
    
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
        await _reader.DisposeAsync().ConfigureAwait(false);
    }
}