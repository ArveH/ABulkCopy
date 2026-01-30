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
    
    public NpgsqlDataSource DataSource => _pgContext.DataSource;
    
    public async Task ExecuteNonQueryAsync(string sqlString, CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
    }

    public Task ExecuteNonQueryAsync(DbCommand command, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> ExecuteScalarAsync(string sqlString, CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        return await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);
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

    public async Task<TReturn?> ExecuteQueryAsync<TReturn>(
        string sqlString,
        IEnumerable<(string name, object value)> parameters,
        Func<IDbRawReader, Task<TReturn?>> func,
        CancellationToken ct)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        foreach (var valueTuple in parameters)
        {
            cmd.Parameters.AddWithValue(valueTuple.name, valueTuple.value);
        }
        
        await using var dbRawReader = _pgRawFactory.CreateReader(
            await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false));

        return await func(dbRawReader);
    }

    public async Task ExecuteReaderAsync(DbCommand command, Action<IDbRawReader> readFunc, CancellationToken ct)
    {
        await using var dbRawReader = _pgRawFactory.CreateReader(
            await command.ExecuteReaderAsync(ct).ConfigureAwait(false));

        if (dbRawReader == null)
        {
            throw new InvalidOperationException("Failed to create IDbRawReader from DbDataReader.");
        }

        while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            readFunc(dbRawReader);
        }
    }

    public async Task ExecuteReaderAsync(DbCommand command, Func<IDbRawReader, Task> readFunc, CancellationToken ct)
    {
        await using var dbRawReader = _pgRawFactory.CreateReader(
            await command.ExecuteReaderAsync(ct).ConfigureAwait(false));

        if (dbRawReader == null)
        {
            throw new InvalidOperationException("Failed to create IDbRawReader from DbDataReader.");
        }

        while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
        {
            await readFunc(dbRawReader).ConfigureAwait(false);
        }
    }
}