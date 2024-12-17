namespace ABulkCopy.APostgres.DbRaw;

public class PgRawCommand : IPgRawCommand
{
    private readonly IPgContext _pgContext;
    private readonly IPgRawFactory _pgRawFactory;

    public PgRawCommand(
        IPgContext pgContext,
        IPgRawFactory pgRawFactory)
    {
        _pgContext = pgContext;
        _pgRawFactory = pgRawFactory;
    }
    
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
        Func<IDbRawReader, Task<TReturn?>> func,
        CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var dbRawReader = _pgRawFactory.CreateReader(
            await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false));

        return await func(dbRawReader);
    }

    public async Task<IEnumerable<TReturn>> ExecuteQueryAsync<TReturn>(
        string sqlString,
        Func<IDbRawReader, Task<IEnumerable<TReturn>>> func,
        CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var dbRawReader = _pgRawFactory.CreateReader(
            await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false));

        return await func(dbRawReader);
    }
}