namespace ABulkCopy.APostgres;

public class PgSystemTables : IPgSystemTables
{
    private readonly IDbRawCommand _dbRawCommand;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly IIdentifier _identifier;
    private readonly ILogger _logger;

    public PgSystemTables(
        IDbRawCommand dbRawCommand,
        IQueryBuilderFactory queryBuilderFactory,
        IIdentifier identifier,
        ILogger logger)
    {
        _dbRawCommand = dbRawCommand;
        _queryBuilderFactory = queryBuilderFactory;
        _identifier = identifier;
        _logger = logger.ForContext<PgSystemTables>();
    }

    public async Task<PrimaryKey?> GetPrimaryKeyAsync(
        TableHeader tableHeader, CancellationToken ct)
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
                        $"WHERE tc.constraint_type = 'PRIMARY KEY' AND tc.table_name='{_identifier.AdjustForSystemTable(tableHeader.Name)}'";
        return await _dbRawCommand.ExecuteQueryAsync(
            sqlString,
            async reader =>
            {
                var isSomethingRead = await reader.ReadAsync(ct).ConfigureAwait(false);
                if (!isSomethingRead) return default;

                var pk = new PrimaryKey
                {
                    Name = reader.GetString(1),
                    ColumnNames = [new() { Name = reader.GetString(3) }]
                };

                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    pk.ColumnNames.Add(new OrderColumn { Name = reader.GetString(3) });
                }

                _logger.Information(
                    "Retrieved primary key {@PrimaryKey} for '{tableName}'",
                    pk, tableHeader.Name);
                return pk;
            },
            ct);
    }

    public async Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var sqlString = "select \r\n" +
                        "    n.nspname as \"parent_schema\", \r\n" +
                        "    cl.relname as \"parent_table\", \r\n" +
                        "    con.conname,\r\n" +
                        "    case update_action when 'c' then 'Cascade' when 'n' then 'SetNull' when 'd' then 'SetDefault' else 'NoAction' end case,\r\n" +
                        "    case delete_action when 'c' then 'Cascade' when 'n' then 'SetNull' when 'd' then 'SetDefault' else 'NoAction' end case \r\n" +
                        "from\r\n" +
                        "   (select \r\n" +
                        "        con1.confrelid, \r\n" +
                        "        con1.conrelid,\r\n" +
                        "        con1.conname,\r\n" +
                        "        con1.confupdtype as \"update_action\",\r\n" +
                        "        con1.confdeltype as \"delete_action\"" +
                        "    from \r\n" +
                        "        pg_class cl\r\n" +
                        "        join pg_constraint con1 on con1.conrelid = cl.oid\r\n" +
                        "    where\r\n" +
                       $"        cl.relname = '{_identifier.AdjustForSystemTable(tableHeader.Name)}'\r\n" +
                        "        and con1.contype = 'f'\r\n" +
                        "   ) con\r\n" +
                        "   join pg_class cl on\r\n" +
                        "       cl.oid = con.confrelid\r\n" +
                        "   join pg_namespace n on\r\n" +
                        "       n.oid = cl.relnamespace\r\n";
        return await _dbRawCommand.ExecuteQueryAsync<ForeignKey>(
            sqlString,
            async reader =>
            {
                var foreignKeys = new List<ForeignKey>();
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    var constraintName = reader.GetString(2);
                    var columns = (await GetForeignKeyColumnsAsync(constraintName, ct).ConfigureAwait(false)).ToList();
                    var fk = new ForeignKey
                    {
                        ColumnNames = columns.Select(c => c.child).ToList(),
                        SchemaReference = reader.GetString(0),
                        TableReference = reader.GetString(1),
                        ColumnReferences = columns.Select(c => c.parent).ToList(),
                        ConstraintName = constraintName,
                        UpdateAction = (UpdateAction)Enum.Parse(typeof(UpdateAction), reader.GetString(3), true),
                        DeleteAction = (DeleteAction)Enum.Parse(typeof(DeleteAction), reader.GetString(4), true)
                    };
                    foreignKeys.Add(fk);
                    _logger.Verbose("Added foreign key: {ForeignKey}", fk.ConstraintName);
                }

                return foreignKeys;
            },
            ct);
    }

    public async Task<uint?> GetIdentityOidAsync(
        string tableName, string columnName, CancellationToken ct)
    {
        var seqName = await GetOwnedSequenceNameAsync(tableName, columnName, ct).ConfigureAwait(false);
        if (seqName == null)
        {
            return null;
        }

        var sqlString =
            $"select oid from pg_class where relkind = 'S' and " +
            $"relname = '{_identifier.AdjustForSystemTable(seqName.TrimSchema())}'";
        var oid = await _dbRawCommand.ExecuteScalarAsync(sqlString, ct).ConfigureAwait(false);
        if (oid == null || oid == DBNull.Value)
        {
            _logger.Error("Can't get oid for sequence '{SequenceName}'", seqName);
            return null;
        }

        return (uint?)oid;
    }

    public async Task ResetIdentityAsync(
        string tableName, string columnName, CancellationToken ct)
    {
        var oid = await GetIdentityOidAsync(tableName, columnName, ct).ConfigureAwait(false);
        if (oid == null)
        {
            throw new SqlNullValueException("Sequence not found");
        }

        var qb = _queryBuilderFactory.GetQueryBuilder();
        qb.AppendLine("select setval(");
        qb.AppendLine($"{oid}, ");
        qb.Append("(select max(");
        qb.AppendIdentifier(columnName);
        qb.Append(") from ");
        qb.AppendIdentifier(tableName);
        qb.AppendLine(") )");

        await _dbRawCommand.ExecuteScalarAsync(qb.ToString(), ct);
    }

    public async Task<string?> GetOwnedSequenceNameAsync(
        string tableName, string columnName, CancellationToken ct)
    {
        var sqlString = 
            $"select pg_get_serial_sequence('{_identifier.AdjustForSystemTable(tableName)}', '{_identifier.AdjustForSystemTable(columnName)}')";

        var seqName = await _dbRawCommand.ExecuteScalarAsync(sqlString, ct).ConfigureAwait(false);
        if (seqName == null || seqName == DBNull.Value)
        {
            _logger.Error("Can't get sequence name for '{TableName}'.'{ColumnName}'",
                tableName, columnName);
            return null;
        }

        return (string?)seqName;
    }

    private async Task<IEnumerable<(string child, string parent)>> GetForeignKeyColumnsAsync(
        string constraintName,
        CancellationToken ct)
    {
        var sqlString = "select \r\n" +
                        "    att2.attname as \"child_column\", \r\n" +
                        "    att.attname as \"parent_column\"\r\n" +
                        "from\r\n" +
                        "   (select \r\n" +
                        "        unnest(con1.conkey) as \"parent\", \r\n" +
                        "        unnest(con1.confkey) as \"child\", \r\n" +
                        "        con1.confrelid, \r\n" +
                        "        con1.conrelid\r\n" +
                        "    from \r\n" +
                        "        pg_constraint con1 \r\n" +
                        "    where\r\n" +
                       $"        con1.conname = '{constraintName}'\r\n" +
                        "   ) con\r\n" +
                        "   join pg_attribute att on\r\n" +
                        "       att.attrelid = con.confrelid and att.attnum = con.child\r\n" +
                        "   join pg_attribute att2 on\r\n" +
                        "       att2.attrelid = con.conrelid and att2.attnum = con.parent";
        return await _dbRawCommand.ExecuteQueryAsync<(string child, string parent)>(
            sqlString,
            async reader =>
            {
                var columns = new List<(string, string)>();
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    columns.Add((reader.GetString(0), reader.GetString(1)));
                }

                return columns;
            },
            ct);
    }
}