namespace ABulkCopy.APostgres;

public class PgSystemTables : IPgSystemTables
{
    private readonly IPgRawCommand _rawCommand;
    private readonly IPgColumnFactory _columnFactory;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly IIdentifier _identifier;
    private readonly ILogger _logger;

    public PgSystemTables(
        IPgRawCommand rawCommand,
        IPgColumnFactory columnFactory,
        IQueryBuilderFactory queryBuilderFactory,
        IIdentifier identifier,
        ILogger logger)
    {
        _rawCommand = rawCommand;
        _columnFactory = columnFactory;
        _queryBuilderFactory = queryBuilderFactory;
        _identifier = identifier;
        _logger = logger.ForContext<PgSystemTables>();
    }

    public async Task<IEnumerable<SchemaTableTuple>> GetFullTableNamesAsync(
        string schemaNames, string searchString, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        if (string.IsNullOrWhiteSpace(searchString))
        {
            _logger.Information("Reading all tables");

            command.CommandText =
            "SELECT table_schema, table_name\n" +
            "FROM information_schema.tables\n" +
            "WHERE table_type = 'BASE TABLE'\n" +
            schemaNames.PgAddSchemaFilter() +
            "ORDER BY table_schema, table_name";
        }
        else
        {
            _logger.Information("Reading tables where search string is '{SearchString}'", searchString);

            command.CommandText =
                "SELECT table_schema, table_name\n" +
                "FROM information_schema.tables\n" +
                "WHERE table_type = 'BASE TABLE'\n" +
                "   AND table_name ILIKE @SearchString\r\n" +
                schemaNames.PgAddSchemaFilter() +
                "ORDER BY table_schema, table_name";
            command.Parameters.AddWithValue("@SearchString", searchString);
        }

        var fullNames = new List<(string, string)>();
        await _rawCommand.ExecuteReaderAsync(command, reader =>
        {
            fullNames.Add((reader.GetString(0), reader.GetString(1)));
        }, ct).ConfigureAwait(false);

        _logger.Information("Found {NumberOfTables} tables.", fullNames.Count);
        return fullNames;
    }

    public async Task<TableHeader?> GetTableHeaderAsync(
        string schemaName, string tableName, CancellationToken ct)
    {
        var result = await GetIdAndLocationAsync(schemaName, tableName, ct).ConfigureAwait(false);
        if (!result.HasValue)
        {
            _logger.Warning("Table Id and Location not found for '{SchemaName}.{TableName}'", tableName, schemaName);
            return null;
        }

        var tableHeader = new TableHeader
        {
            Id = (int)result.Value.id,
            Schema = schemaName,
            Name = tableName,
            Location = result.Value.location
        };
        
        var identityColumns = await GetIdentityColumnsAsync(tableHeader.Id, ct).ConfigureAwait(false);
        tableHeader.Identity = identityColumns.FirstOrDefault();
        // TODO: Remove Identity from TableHeader
        if (tableHeader.Identity == null)
        {
            _logger.Debug("No identity columns found for '{SchemaName}.{TableName}'", schemaName, tableName);
        }

        _logger.Information("Retrieved table header {@TableHeader} for '{SchemaName}.{TableName}'",
            tableHeader, schemaName, tableName);
        return tableHeader;
    }

    public async Task<IEnumerable<IColumn>> GetTableColumnInfoAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        command.CommandText = StaticQueries.GetColumnInfo();
        command.Parameters.AddWithValue("@TableId", tableHeader.Id);

        var columns = new List<IColumn>();
        await _rawCommand.ExecuteReaderAsync(command, reader =>
        {
            var column = _columnFactory.Create(
                reader.GetInt16(0), // ColumnId
                reader.GetString(1), // Name
                reader.GetString(2), // DataType
                reader.IsDBNull(3) ? 0 : reader.GetInt32(3), // Length
                reader.IsDBNull(4) ? null : reader.GetInt32(4), // Precision
                reader.IsDBNull(5) ? null : reader.GetInt32(5), // Scale
                reader.GetBoolean(6), // IsNullable
                reader.IsDBNull(7) ? null : reader.GetString(7) // CollationName
            );
            column.DefaultConstraint = reader.IsDBNull(8) ? null : new() {Definition = reader.GetString(8)};

            columns.Add(column);
        }, ct).ConfigureAwait(false);
        await SetIdentityProperties(tableHeader, columns, ct);

        _logger.Information("Retrieved {ColumnCount} columns for '{SchemaName}.{TableName}'",
            columns.Count, tableHeader.Schema, tableHeader.Name);
        return columns;
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
        return await _rawCommand.ExecuteQueryAsync(
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
                    "Retrieved primary key {@PrimaryKey} for '{TableName}'",
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
        return await _rawCommand.ExecuteQueryAsync<ForeignKey>(
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

        await _rawCommand.ExecuteScalarAsync(qb.ToString(), ct);
    }

    public async Task<IEnumerable<IndexDefinition>> GetIndexesAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        command.CommandText = StaticQueries.GetIndexInfoWithoutColumnInfo();
        command.Parameters.AddWithValue("@SchemaName", _identifier.AdjustForSystemTable(tableHeader.Schema));
        command.Parameters.AddWithValue("@TableName", _identifier.AdjustForSystemTable(tableHeader.Name));

        var indexes = new List<IndexDefinition>();
        await _rawCommand.ExecuteReaderAsync(
            command,
            ReadAllIndexesForTable(indexes, ct),
            ct);

        foreach (var indexDefinition in indexes)
        {
            await AddIndexColumnInformationAsync(indexDefinition, ct);
        }
        
        _logger.Information(
            "Retrieved {IndexCount} {IndexPlural} for table '{TableName}'",
            indexes.Count, "index".Plural(indexes.Count), tableHeader.Name);

        return indexes;
    }

    private async Task AddIndexColumnInformationAsync(IndexDefinition indexDefinition, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        command.CommandText = StaticQueries.GetColumnInfoForIndex();
        command.Parameters.AddWithValue("@IndexRelId", indexDefinition.Header.Id);
        
         await _rawCommand.ExecuteReaderAsync(
            command,
            reader =>
            {
                var columnName = reader.IsDBNull(0) ? null : reader.GetString(0);
                var columnPosition = reader.GetInt32(1);
                var descOrder = !reader.IsDBNull(2) && reader.GetBoolean(2);
                _ = !reader.IsDBNull(3) && reader.GetBoolean(3);
                var columnExpression = reader.IsDBNull(4) ? null : reader.GetString(4);

                var indexColumn = new IndexColumn
                {
                    Name = columnName ?? columnExpression ?? $"expr_{columnPosition}",
                    Direction = descOrder ? Direction.Descending : Direction.Ascending
                };
                indexDefinition.Columns.Add(indexColumn);
                return Task.CompletedTask;
            },
            ct);
    }

    private Func<IDbRawReader, Task> ReadAllIndexesForTable(
        List<IndexDefinition> indexes, 
        CancellationToken ct)
    {
        return async reader =>
        {
            do
            {
                var indexDefinition = ReadSingleIndexInformation(reader);
                indexes.Add(indexDefinition);
            } while (await reader.ReadAsync(ct).ConfigureAwait(false));
        };
    }

    private IndexDefinition ReadSingleIndexInformation(IDbRawReader reader)
    {
        var tableOid = reader.GetUInt32(0);
        var indexName = reader.GetString(1);
        var indexOid = reader.GetUInt32(2);
        var accessMethod = reader.GetString(3);
        var isUnique = reader.GetBoolean(4);
        var isClustered = reader.GetBoolean(5);

        var type = (IndexType)Enum.Parse(typeof(IndexType), accessMethod, true);
        var indexDef = new IndexDefinition
        {
            Header = new IndexHeader
            {
                Id = indexOid,
                Name = indexName,
                TableId = tableOid,
                Location = "",
                IsUnique = isUnique,
                Type = type,
                IsClustered = isClustered,
            }
        };
        _logger.Verbose("Added index: {IndexName}", indexDef.Header.Name);
        return indexDef;
    }

    private async Task SetIdentityProperties(TableHeader tableHeader, List<IColumn> columns, CancellationToken ct)
    {
        var identityColumns = (await GetIdentityColumnsAsync(tableHeader.Id, ct).ConfigureAwait(false)).ToList();

        foreach (var column in columns)
        {
            var identity = identityColumns.FirstOrDefault(ic => ic.ColumnName == column.Name);
            if (identity != null)
            {
                column.Identity = identity;
            }
        }
    }

    private async Task<IEnumerable<Identity>> GetIdentityColumnsAsync(
        long tableId, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        command.CommandText = StaticQueries.GetIdentityColumns();
        command.Parameters.AddWithValue("@TableId", tableId);

        var identities = new List<Identity>();
        await _rawCommand.ExecuteReaderAsync(command, reader =>
        {
            var identity = new Identity(
                reader.GetString(0), 
                reader.GetInt64(1), 
                reader.GetInt64(2),
                reader.GetChar(3));
            identities.Add(identity);
        }, ct).ConfigureAwait(false);

        return identities;
    }

    private async Task<uint?> GetIdentityOidAsync(
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
        var oid = await _rawCommand.ExecuteScalarAsync(sqlString, ct).ConfigureAwait(false);
        if (oid == null || oid == DBNull.Value)
        {
            _logger.Error("Can't get oid for sequence '{SequenceName}'", seqName);
            return null;
        }

        return (uint?)oid;
    }

    private async Task<string?> GetOwnedSequenceNameAsync(
        string tableName, string columnName, CancellationToken ct)
    {
        var sqlString = 
            $"select pg_get_serial_sequence('{_identifier.AdjustForSystemTable(tableName)}', '{_identifier.AdjustForSystemTable(columnName)}')";

        var seqName = await _rawCommand.ExecuteScalarAsync(sqlString, ct).ConfigureAwait(false);
        if (seqName == null || seqName == DBNull.Value)
        {
            _logger.Error("Can't get sequence name for '{TableName}'.'{ColumnName}'",
                tableName, columnName);
            return null;
        }

        return (string?)seqName;
    }

    private async Task<(uint id, string location)?> GetIdAndLocationAsync(
        string schemaName, string tableName, CancellationToken ct)
    {
        await using var command = _rawCommand.DataSource.CreateCommand();
        command.CommandText = StaticQueries.GetTableIdAndLocation();
        command.Parameters.AddWithValue("@SchemaName", _identifier.AdjustForSystemTable(schemaName));
        command.Parameters.AddWithValue("@TableName", _identifier.AdjustForSystemTable(tableName));

        (uint id, string location)? result = null;
        await _rawCommand.ExecuteReaderAsync(command, reader =>
        {
            result = new ValueTuple<uint, string>(reader.GetUInt32(0), reader.GetString(1));
        }, ct).ConfigureAwait(false);

        if (result == null)
        {
            _logger.Warning("Table '{TableName}' not found in schema '{SchemaName}'", tableName, schemaName);
        }

        return result;
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
        return await _rawCommand.ExecuteQueryAsync<(string child, string parent)>(
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