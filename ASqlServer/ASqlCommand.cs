namespace ASqlServer;

public class ASqlCommand : IASqlCommand
{
    private readonly ILogger _logger;

    public ASqlCommand(ILogger logger)
    {
        _logger = logger;
    }

    public required string ConnectionString { get; init; }

    public async IAsyncEnumerable<string> GetTableNames(string searchString)
    {
        _logger.Information("Connecting to database...");

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("select name from sys.objects\r\n" +
                           "where type = 'U'\r\n" +
                           "and name like @SearchString\r\n" +
                           "order by name", 
                connection);
        command.Parameters.AddWithValue("@SearchString", searchString);
        await using var reader = await command.ExecuteReaderAsync();
        _logger.Information("Connected to '{dbName}'.", connection.Database);
        var counter = 0;
        while (await reader.ReadAsync())
        {
            yield return reader.GetString(0);
            counter++;
        }
        _logger.Information("Returned {numberOfTables} tables.", counter);
    }
}