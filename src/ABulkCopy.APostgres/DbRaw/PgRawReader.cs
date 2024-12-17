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
}