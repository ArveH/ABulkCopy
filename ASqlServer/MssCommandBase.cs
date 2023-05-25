﻿namespace ASqlServer;

public class MssCommandBase
{
    private readonly IDbContext _dbContext;

    public MssCommandBase(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteReader(
        SqlCommand command,
        Action<SqlDataReader> readFunc)
    {
        var connection = new SqlConnection(_dbContext.ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            command.Connection = connection;
            await connection.OpenAsync().ConfigureAwait(false);
            await using (command.ConfigureAwait(false))
            {
                await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    readFunc(reader);
                }
            }
        }
    }
}