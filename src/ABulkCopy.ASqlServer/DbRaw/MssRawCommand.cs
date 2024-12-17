namespace ABulkCopy.ASqlServer.DbRaw;

public class MssRawCommand : IMssRawCommand
{
        private readonly IDbContext _dbContext;
        private readonly IMssRawFactory _mssRawFactory;

        public MssRawCommand(
        IDbContext dbContext,
        IMssRawFactory mssRawFactory)
        {
            _dbContext = dbContext;
            _mssRawFactory = mssRawFactory;
        }

    public async Task ExecuteReaderAsync(
        SqlCommand command,
        Action<IDbRawReader> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(_dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var dbRawReader = 
                    _mssRawFactory.CreateReader(await command.ExecuteReaderAsync(ct).ConfigureAwait(false));
                while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
                {
                    readFunc(dbRawReader);
                }
            }
        }
    }

    public async Task ExecuteReaderAsync(
        SqlCommand command,
        Func<IDbRawReader, Task> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(_dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var dbRawReader = 
                    _mssRawFactory.CreateReader(await command.ExecuteReaderAsync(ct).ConfigureAwait(false));
                while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
                {
                    await readFunc(dbRawReader).ConfigureAwait(false);
                }
            }
        }
    }
    
    public async Task<TReturn?> ExecuteQueryAsync<TReturn>(
        string sqlString, 
        Func<IDbRawReader, Task<TReturn?>> func, 
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_dbContext.ConnectionString);
        await using var command = new SqlCommand(sqlString, connection);
        await connection.OpenAsync(ct).ConfigureAwait(false);
        await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
        return await func(_mssRawFactory.CreateReader(reader));
    }

    public async Task<IEnumerable<TReturn>> ExecuteQueryAsync<TReturn>(
        string sqlString, 
        Func<IDbRawReader, Task<IEnumerable<TReturn>>> func, 
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(_dbContext.ConnectionString);
        await using var command = new SqlCommand(sqlString, connection);
        await connection.OpenAsync(ct).ConfigureAwait(false);
        await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
        return await func(_mssRawFactory.CreateReader(reader));
    }

    public async Task ExecuteNonQueryAsync(
        string sqlString,
        CancellationToken ct)
    {
        await using var sqlConnection = new SqlConnection(_dbContext.ConnectionString);
        await sqlConnection.OpenAsync(ct);
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync(ct);
    }

    public async Task<object?> ExecuteScalarAsync(string sqlString, CancellationToken ct)
    {
        await using var sqlConnection = new SqlConnection(_dbContext.ConnectionString);
        await sqlConnection.OpenAsync(ct);
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        return await sqlCommand.ExecuteScalarAsync(ct);
    }
}