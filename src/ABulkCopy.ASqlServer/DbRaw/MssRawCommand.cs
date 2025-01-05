namespace ABulkCopy.ASqlServer.DbRaw;

public class MssRawCommand(
    IDbContext dbContext,
    IMssRawFactory mssRawFactory) : IMssRawCommand
{
    public async Task ExecuteReaderAsync(
        DbCommand command,
        Action<IDbRawReader> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var dbRawReader = 
                    mssRawFactory.CreateReader(await command.ExecuteReaderAsync(ct).ConfigureAwait(false));
                while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
                {
                    readFunc(dbRawReader);
                }
            }
        }
    }

    public async Task ExecuteReaderAsync(
        DbCommand command,
        Func<IDbRawReader, Task> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var dbRawReader =
                    mssRawFactory.CreateReader(await command.ExecuteReaderAsync(ct).ConfigureAwait(false));
                while (await dbRawReader.ReadAsync(ct).ConfigureAwait(false))
                {
                    await readFunc(dbRawReader).ConfigureAwait(false);
                }
            }
        }
    }
    
    // TODO: Rename to scalar?
    public async Task<TReturn?> ExecuteQueryAsync<TReturn>(
        string sqlString, 
        Func<IDbRawReader, Task<TReturn?>> func, 
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(dbContext.ConnectionString);
        await using var command = new SqlCommand(sqlString, connection);
        await connection.OpenAsync(ct).ConfigureAwait(false);
        await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
        return await func(mssRawFactory.CreateReader(reader)).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TReturn>> ExecuteQueryAsync<TReturn>(
        string sqlString, 
        Func<IDbRawReader, Task<IEnumerable<TReturn>>> func, 
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(dbContext.ConnectionString);
        await using var command = new SqlCommand(sqlString, connection);
        await connection.OpenAsync(ct).ConfigureAwait(false);
        await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
        return await func(mssRawFactory.CreateReader(reader)).ConfigureAwait(false);
    }

    public async Task ExecuteNonQueryAsync(
        string sqlString,
        CancellationToken ct)
    {
        await using var sqlConnection = new SqlConnection(dbContext.ConnectionString);
        await sqlConnection.OpenAsync(ct).ConfigureAwait(false);
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        await sqlCommand.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
    }
    
    public async Task ExecuteNonQueryAsync(
        DbCommand command,
        CancellationToken ct)
    {
        await using var sqlConnection = new SqlConnection(dbContext.ConnectionString);
        await sqlConnection.OpenAsync(ct).ConfigureAwait(false);
        command.Connection = sqlConnection;
        await command.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
    }

    public async Task<object?> ExecuteScalarAsync(string sqlString, CancellationToken ct)
    {
        await using var sqlConnection = new SqlConnection(dbContext.ConnectionString);
        await sqlConnection.OpenAsync(ct).ConfigureAwait(false);
        await using var sqlCommand = new SqlCommand(sqlString, sqlConnection);
        return await sqlCommand.ExecuteScalarAsync(ct).ConfigureAwait(false);
    }
}