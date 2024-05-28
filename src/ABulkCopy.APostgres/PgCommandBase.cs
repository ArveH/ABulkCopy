namespace ABulkCopy.APostgres;

public class PgCommandBase
{
    private readonly IPgContext _pgContext;

    public PgCommandBase(IPgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public int MaxIdentifierLength => _pgContext.MaxIdentifierLength;

    public async Task ExecuteNonQueryAsync(string sqlString, CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
    }

    public async Task<object?> ExecuteScalarAsync(string sqlString, CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        return await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);
    }

    public async Task<TReturn?> ExecuteQueryAsync<TReturn>(
        string sqlString,
        Func<NpgsqlDataReader, Task<TReturn?>> func,
        CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

        return await func(reader);
    }

    public async Task<IEnumerable<TReturn>> ExecuteQueryAsync<TReturn>(
        string sqlString,
        Func<NpgsqlDataReader, Task<IEnumerable<TReturn>>> func,
        CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

        return await func(reader);
    }
}