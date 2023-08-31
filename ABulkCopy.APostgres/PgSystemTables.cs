﻿namespace ABulkCopy.APostgres;

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

    public async Task<IEnumerable<ForeignKey>> GetForeignKeys(TableHeader tableHeader)
    {
        var sqlString = "select \r\n" +
                        "    att2.attname as \"child_column\", \r\n" +
                        "    cl.relname as \"parent_table\", \r\n" +
                        "    att.attname as \"parent_column\",\r\n" +
                        "    conname,\r\n" +
                        "    case update_action when 'c' then 'Cascade' when 'n' then 'SetNull' when 'd' then 'SetDefault' else 'NoAction' end case,\r\n" +
                        "    case delete_action when 'c' then 'Cascade' when 'n' then 'SetNull' when 'd' then 'SetDefault' else 'NoAction' end case\r\n" +
                        "from\r\n" +
                        "   (select \r\n" +
                        "        unnest(con1.conkey) as \"parent\", \r\n" +
                        "        unnest(con1.confkey) as \"child\", \r\n" +
                        "        con1.confrelid, \r\n" +
                        "        con1.conrelid,\r\n" +
                        "        con1.conname,\r\n" +
                        "        con1.confupdtype as \"update_action\",\r\n" +
                        "        con1.confdeltype as \"delete_action\"\r\n" +
                        "    from \r\n" +
                        "        pg_class cl\r\n" +
                        "        join pg_namespace ns on cl.relnamespace = ns.oid\r\n" +
                        "        join pg_constraint con1 on con1.conrelid = cl.oid\r\n" +
                        "    where\r\n" +
                       $"        cl.relname = '{tableHeader.Name}'\r\n" +
                        "        and ns.nspname = 'public'\r\n" +
                        "        and con1.contype = 'f'\r\n" +
                        "   ) con\r\n" +
                        "   join pg_attribute att on\r\n" +
                        "       att.attrelid = con.confrelid and att.attnum = con.child\r\n" +
                        "   join pg_class cl on\r\n" +
                        "       cl.oid = con.confrelid\r\n" +
                        "   join pg_attribute att2 on\r\n" +
                        "       att2.attrelid = con.conrelid and att2.attnum = con.parent";
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await using var reader = await cmd.ExecuteReaderAsync();

        var isSomethingRead = await reader.ReadAsync();
        if (!isSomethingRead) return new List<ForeignKey>();

        var foreignKeys = new List<ForeignKey>();
        while (await reader.ReadAsync())
        {
            var fk = new ForeignKey
            {
                ColName = reader.GetString(0),
                TableReference = reader.GetString(1),
                ColumnReference = reader.GetString(2),
                Name = reader.GetString(3),
                UpdateAction = (UpdateAction)Enum.Parse(typeof(UpdateAction), reader.GetString(4), true),
                DeleteAction = (DeleteAction)Enum.Parse(typeof(DeleteAction), reader.GetString(5), true)
            };
            foreignKeys.Add(fk);
            _logger.Verbose("Added foreign key: {ForeignKey}", fk.Name);
        }

        return foreignKeys;
    }
}