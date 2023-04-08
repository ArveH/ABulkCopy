﻿namespace ASqlServer;

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
            new SqlCommand("SELECT o.object_id AS id, " +
                           "       OBJECT_SCHEMA_NAME(o.object_id) AS [schema], " +
                           "       f.name AS segname,\r\n" +
                           "       IDENT_SEED(@TableName) AS seed,\r\n" +
                           "       IDENT_INCR(@TableName) AS increment " +
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
                Identity? identity = null;
                if (!reader.IsDBNull(3))
                {
                    identity = new Identity
                    {
                        Seed = Convert.ToInt32(reader.GetDecimal(3)),
                        Increment = Convert.ToInt32(reader.GetDecimal(4))
                    };
                }

                tableDefinitions.Add(new TableDefinition
                {
                    Id = reader.GetInt32(0),
                    Schema = reader.GetString(1),
                    Name = tableName,
                    Location = reader.GetString(2),
                    Identity = identity
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

    public async Task<IEnumerable<ColumnDefinition>> GetColumnInfo(TableDefinition tableDef)
    {
        var command =
            new SqlCommand("SELECT c.column_id,\r\n" +
                           "       c.name, \r\n" +
                           "       t.name AS t_name, \r\n" +
                           "       c.max_length AS [length], \r\n" +
                           "       c.precision AS prec, \r\n" +
                           "       c.scale AS scale, \r\n" +
                           "       c.is_nullable, \r\n" +
                           "       c.collation_name AS collation, \r\n" +
                           "       OBJECT_NAME(c.default_object_id) AS default_name, \r\n" +
                           "       OBJECT_DEFINITION(c.default_object_id) AS default_definition, \r\n" +
                           "       c.is_identity \r\n" +
                           "FROM   sys.columns c \r\n" +
                           "       JOIN sys.types t \r\n" +
                           "         ON c.user_type_id = t.user_type_id \r\n" +
                           "WHERE  c.object_id = OBJECT_ID('AllTypes') \r\n" +
                           "ORDER  BY c.column_id");
        command.Parameters.AddWithValue("@TableName", tableDef.Name);

        var columnDefinitions = new List<ColumnDefinition>();
        await ExecuteReader(command, reader =>
        {
            DefaultDefinition? defaultDef = null;
            if (!reader.IsDBNull(8))
            {
                defaultDef = new DefaultDefinition
                {
                    Name = reader.GetString(8),
                    Definition = reader.GetString(9)
                };
            }

            var columnDef = new ColumnDefinition
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                DataType = reader.GetString(2),
                Length = reader.GetInt16(3),
                Precision = reader.GetByte(4),
                Scale = reader.GetByte(5),
                IsNullable = reader.GetBoolean(6),
                Collation = reader.IsDBNull(7) ? null : reader.GetString(7),
                DefaultConstraint = defaultDef,
                Identity = tableDef.Identity
            };
            columnDefinitions.Add(columnDef);

            _logger.Verbose("Added column: {@columnDefinition}", 
                columnDefinitions.Last());
        });
        _logger.Information(
            "Retrieved column info for {colCount} columns on '{tableName}'",
            columnDefinitions.Count,
            tableDef.Name);
        return columnDefinitions;
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