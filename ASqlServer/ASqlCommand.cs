namespace ASqlServer;

public class ASqlCommand : IASqlCommand
{
    private readonly ILogger _logger;

    public ASqlCommand(ILogger logger)
    {
        _logger = logger;
    }

    public required string ConnectionString { get; init; }

    public async Task<IEnumerable<string>> GetTableNames(string searchString)
    {
        _logger.Information("Reading tables '{searchString}'", searchString);

        var command =
            new SqlCommand("SELECT name FROM sys.objects WITH(NOLOCK)\r\n" +
                           " WHERE type = 'U'\r\n" +
                           "   AND name LIKE @SearchString\r\n" +
                           "ORDER BY name");
        command.Parameters.AddWithValue("@SearchString", searchString);

        var tableNames = new List<string>();
        await ExecuteReader(command, reader =>
        {
            tableNames.Add(reader.GetString(0));
        });

        _logger.Information("Found {numberOfTables} tables.", tableNames.Count);
        return tableNames;
    }

    public async Task<TableDefinition?> GetTableInfo(string tableName)
    {
        var command =
            new SqlCommand("SELECT o.object_id AS id, OBJECT_SCHEMA_NAME(o.object_id) AS [schema], f.name AS segname \r\n" +
                           "FROM   sys.objects o WITH(NOLOCK)\r\n" +
                           "INNER JOIN sys.indexes i WITH(NOLOCK)\r\n" +
                           "    ON i.object_id = o.object_id\r\n" +
                           "INNER JOIN sys.filegroups f WITH(NOLOCK)\r\n" +
                           "    ON i.data_space_id = f.data_space_id\r\n" +
                           "WHERE o.type = 'U'\r\n" +
                           "  AND (i.index_id = 0 OR i.index_id = 1)\r\n" +
                           "  AND o.name = @TableName\r\n");
        command.Parameters.AddWithValue("@TableName", tableName);

        var tableDefinitions = new List<TableDefinition>();
        await ExecuteReader(command, reader =>
            {
                tableDefinitions.Add(new TableDefinition
                {
                    Id = reader.GetInt32(0),
                    Schema = reader.GetString(1),
                    Name = tableName,
                    Location = reader.GetString(2)
                });
            });
        if (tableDefinitions.Count == 1)
        {
            _logger.Information(
                "Retrieved table info for '{tableName}': Id={id}, Schema='{schema}', Location='{location}'",
                tableName,
                tableDefinitions[0].Id,
                tableDefinitions[0].Schema,
                tableDefinitions[0].Location);
            return tableDefinitions[0];
        }

        _logger.Warning($"Table information for table '{tableName}' was not found");
        return null;
    }

    public async Task ExecuteReader(
        SqlCommand command,
        Action<SqlDataReader> readFunc)
    {
        var connection = new SqlConnection(ConnectionString);
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