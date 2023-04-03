using System.Diagnostics.Metrics;
using System.Reflection;

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
        _logger.Information("Get table names");

        var connection = new SqlConnection(ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync();
            var command =
                new SqlCommand("select name from sys.objects with(nolock)\r\n" +
                               "where type = 'U'\r\n" +
                               "and name like @SearchString\r\n" +
                               "order by name",
                    connection);
            command.Parameters.AddWithValue("@SearchString", searchString);
            await using (command.ConfigureAwait(false))
            {
                await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                _logger.Information("Reading tables from '{dbName}'", connection.Database);
                var counter = 0;

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    yield return reader.GetString(0);
                    counter++;
                }
                _logger.Information("Found {numberOfTables} tables.", counter);
            }
        }
    }

    public async Task<TableDefinition?> GetTableInfo(string tableName)
    {
        var connection = new SqlConnection(ConnectionString);
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync();
            var command =
                new SqlCommand("SELECT o.object_id AS id, OBJECT_SCHEMA_NAME(o.object_id) AS [schema], f.name AS segname \r\n" +
                               "FROM   sys.objects o WITH(NOLOCK)\r\n" +
                               "INNER JOIN sys.indexes i WITH(NOLOCK)\r\n" +
                               "    ON i.object_id = o.object_id\r\n" +
                               "INNER JOIN sys.filegroups f WITH(NOLOCK)\r\n" +
                               "    ON i.data_space_id = f.data_space_id\r\n" +
                               "WHERE o.type = 'U'\r\n" +
                               "  AND (i.index_id = 0 OR i.index_id = 1)\r\n" +
                               "  AND o.name = @TableName\r\n",
                    connection);
            command.Parameters.AddWithValue("@TableName", tableName);
            await using (command.ConfigureAwait(false))
            {
                await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var tableDefinition = new TableDefinition
                    {
                        Id = reader.GetInt32(0),
                        Schema = reader.GetString(1),
                        Name = tableName,
                        Location = reader.GetString(2)
                    };
                    _logger.Information(
                        "Retrieved table info for '{tableName}': Id={id}, Schema='{schema}', Location='{location}'",
                        tableName, 
                        tableDefinition.Id, 
                        tableDefinition.Schema, 
                        tableDefinition.Location);
                    return tableDefinition;
                }

                _logger.Warning("Couldn't get table definition for '{tableName}'", tableName);
                return null;
            }
        }
    }
}