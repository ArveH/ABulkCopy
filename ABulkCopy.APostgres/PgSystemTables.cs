namespace ABulkCopy.APostgres;

public class PgSystemTables : IPgSystemTables
{
    private readonly IPgContext _pgContext;
    private readonly ILogger _logger;

    public PgSystemTables(
        IPgContext pgContext,
        ILogger logger)
    {
        _pgContext = pgContext;
        _logger = logger.ForContext<PgSystemTables>();
    }

    public async Task<PrimaryKey?> GetPrimaryKey(TableHeader tableHeader)
    {
        var sqlString = "SELECT\r\n" +
                        "    tc.table_schema,\r\n" +
                        "    tc.constraint_name,\r\n" +
                        "    tc.table_name,\r\n" +
                        "    kcu.column_name\r\n" +
                        "FROM\r\n" +
                        "    information_schema.table_constraints AS tc\r\n" +
                        "    JOIN information_schema.key_column_usage AS kcu\r\n" +
                        "    ON tc.constraint_name = kcu.constraint_name\r\n" +
                        "        AND tc.table_schema = kcu.table_schema\r\n" +
                       $"WHERE tc.constraint_type = 'PRIMARY KEY' AND tc.table_name='{tableHeader.Name}'";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var reader = await cmd.ExecuteReaderAsync();

        var isSomethingRead = await reader.ReadAsync();
        if (!isSomethingRead) return null;

        var pk = new PrimaryKey
        {
            Name = reader.GetString(1),
            ColumnNames = new List<OrderColumn>
            {
                new() { Name = reader.GetString(3) }
            }
        };

        while (await reader.ReadAsync())
        {
            pk.ColumnNames.Add(new OrderColumn { Name = reader.GetString(3) });
        }

        _logger.Information(
            "Retrieved primary key {@PrimaryKey} for '{tableName}'",
            pk, tableHeader.Name);
        return pk;
    }

}