namespace ABulkCopy.ASqlServer;

public class MssCommandBase
{
    private readonly IDbContext _dbContext;

    public MssCommandBase(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteReaderAsync(
        SqlCommand command,
        Action<SqlDataReader> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(_dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    readFunc(reader);
                }
            }
        }
    }

    public async Task ExecuteReaderAsync(
        SqlCommand command,
        Func<SqlDataReader, Task> readFunc,
        CancellationToken ct)
    {
        var connection = new SqlConnection(_dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync(ct).ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false);
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    await readFunc(reader).ConfigureAwait(false);
                }
            }
        }
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
}