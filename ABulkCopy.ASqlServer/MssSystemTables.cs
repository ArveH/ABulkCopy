namespace ABulkCopy.ASqlServer;

public class MssSystemTables : MssCommandBase, IMssSystemTables
{
    private readonly IMssColumnFactory _columnFactory;
    private readonly ILogger _logger;

    public MssSystemTables(
        IDbContext dbContext,
        IMssColumnFactory columnFactory,
        ILogger logger)
        : base(dbContext)
    {
        _columnFactory = columnFactory;
        _logger = logger.ForContext<MssSystemTables>();
    }

    public async Task<IEnumerable<string>> GetTableNamesAsync(
        string searchString, CancellationToken ct)
    {
        SqlCommand? command;
        if (string.IsNullOrWhiteSpace(searchString))
        {
            _logger.Information("Reading all tables");

            command =
                new SqlCommand("SELECT name FROM sys.tables WITH(NOLOCK)\r\n" +
                               " WHERE object_id not in (\r\n" +
                               "   SELECT major_id\r\n" +
                               "     FROM sys.extended_properties WITH(NOLOCK)\r\n" +
                               "    WHERE minor_id = 0\r\n" +
                               "      AND class = 1\r\n" +
                               "      AND name = N'microsoft_database_tools_support')\r\n" +
                               "   AND is_ms_shipped = 0\r\n" +
                               "   AND schema_id not in (2, 3, 4, 5)\r\n" + // guest, information_schema, sys, logs
                               "ORDER BY name");
        }
        else
        {
            _logger.Information("Reading tables where search string is '{searchString}'", searchString);

            command =
                new SqlCommand("SELECT name FROM sys.tables WITH(NOLOCK)\r\n" +
                               " WHERE object_id not in (\r\n" +
                               "   SELECT major_id\r\n" +
                               "     FROM sys.extended_properties WITH(NOLOCK)\r\n" +
                               "    WHERE minor_id = 0\r\n" +
                               "      AND class = 1\r\n" +
                               "      AND name = N'microsoft_database_tools_support')\r\n" +
                               "   AND name LIKE @SearchString\r\n" +
                               "   AND is_ms_shipped = 0\r\n" +
                               "   AND schema_id not in (2, 3, 4, 5)\r\n" + // guest, information_schema, sys, logs
                               "ORDER BY name");
            command.Parameters.AddWithValue("@SearchString", searchString);
        }

        var tableNames = new List<string>();
        await ExecuteReaderAsync(command, reader =>
        {
            tableNames.Add(reader.GetString(0));
        }, ct).ConfigureAwait(false);

        _logger.Information("Found {numberOfTables} tables.", tableNames.Count);
        return tableNames;
    }

    public async Task<TableHeader?> GetTableHeaderAsync(
        string tableName, CancellationToken ct)
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

        var tableHeaders = new List<TableHeader>();
        await ExecuteReaderAsync(command, reader =>
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

            tableHeaders.Add(new TableHeader
            {
                Id = reader.GetInt32(0),
                Schema = reader.GetString(1),
                Name = tableName,
                Location = reader.GetString(2),
                Identity = identity
            });
        }, ct).ConfigureAwait(false);
        if (tableHeaders.Count == 1)
        {
            _logger.Information(
                "Retrieved table info for '{tableName}': Id={id}, Schema='{schema}', Location='{location}'",
                tableName,
                tableHeaders[0].Id,
                tableHeaders[0].Schema,
                tableHeaders[0].Location);
            return tableHeaders[0];
        }

        _logger.Warning($"Table information for table '{tableName}' was not found");
        return null;
    }

    public async Task<IEnumerable<IColumn>> GetTableColumnInfoAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var command =
            new SqlCommand("WITH cte AS (\r\n" +
                           "SELECT c.column_id,\r\n" +
                           "       c.name, \r\n" +
                           "       t.name AS t_name, \r\n" +
                           "       c.max_length AS [length], \r\n" +
                           "       c.precision AS prec, \r\n" +
                           "       c.scale AS scale, \r\n" +
                           "       c.is_nullable, \r\n" +
                           "       c.collation_name AS collation, \r\n" +
                           "       c.default_object_id, \r\n" +
                           "       c.is_identity \r\n" +
                           "FROM   sys.columns c WITH(NOLOCK)\r\n" +
                           "JOIN   sys.types t WITH(NOLOCK) ON c.user_type_id = t.user_type_id \r\n" +
                           "WHERE  c.object_id = OBJECT_ID(@TableName) \r\n" +
                           ")\r\n" +
                           "SELECT c.column_id,\r\n" +
                           "       c.name, \r\n" +
                           "       c.t_name, \r\n" +
                           "       c.[length], \r\n" +
                           "       c.prec, \r\n" +
                           "       c.scale, \r\n" +
                           "       c.is_nullable, \r\n" +
                           "       c.collation,\r\n" +
                           "       c.is_identity, \r\n" + // Field no 8
                           "       o.name AS def_name,\r\n" +
                           "       OBJECT_DEFINITION(c.default_object_id) AS [definition],\r\n" +
                           "       d.is_system_named\r\n" +
                           "FROM cte c \r\n" +
                           "LEFT JOIN sys.objects o WITH(NOLOCK) ON (c.default_object_id = o.object_id)\r\n" +
                           "LEFT JOIN sys.default_constraints d WITH(NOLOCK) ON (d.object_id = c.default_object_id)");
        command.Parameters.AddWithValue("@TableName", tableHeader.Name);

        var columnDefinitions = new List<IColumn>();
        await ExecuteReaderAsync(command, reader =>
        {
            var columnDef = _columnFactory.Create(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt16(3),
                reader.GetByte(4),
                reader.GetByte(5),
                reader.GetBoolean(6),
                reader.IsDBNull(7) ? null : reader.GetString(7));

            if (reader.GetBoolean(8))
            {
                if (tableHeader.Identity == null)
                {
                    _logger.Error("Table doesn't have an Identity, but column '{ColumnName}' is_identity = true",
                        columnDef.Name);
                    throw new ArgumentNullException(
                        nameof(tableHeader.Identity),
                        $"Table doesn't have an Identity, but column '{columnDef.Name}' is_identity = true");
                }
                columnDef.Identity = tableHeader.Identity.Clone();
            }
            if (!reader.IsDBNull(9))
            {
                columnDef.DefaultConstraint = new DefaultDefinition
                {
                    Name = reader.GetString(9),
                    Definition = reader.GetString(10),
                    IsSystemNamed = reader.IsDBNull(11) ? false : reader.GetBoolean(11)
                };
            }

            columnDefinitions.Add(columnDef);

            _logger.Verbose("Added column: {@columnDefinition}",
                columnDefinitions.Last());
        }, ct).ConfigureAwait(false);
        _logger.Information(
            "Retrieved column info for {colCount} columns on '{tableName}'",
            columnDefinitions.Count,
            tableHeader.Name);
        return columnDefinitions;
    }

    public async Task<PrimaryKey?> GetPrimaryKeyAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var command =
            new SqlCommand("SELECT c.name, \r\n" +
                           "       index_col(@TableName, ic.index_id, index_column_id) as col_name,\r\n" +
                           "       ic.is_descending_key\r\n" +
                           "FROM sys.key_constraints c\r\n" +
                           "JOIN sys.index_columns ic ON (ic.object_id = c.parent_object_id AND ic.index_id = c.unique_index_id)\r\n" +
                           "WHERE ic.object_id = @TableId\r\n" +
                           "AND c.[type] = 'PK'\r\n" +
                           "ORDER BY ic.index_id, ic.index_column_id");
        command.Parameters.AddWithValue("@TableName", tableHeader.Name);
        command.Parameters.AddWithValue("@TableId", tableHeader.Id);

        PrimaryKey? pk = null;
        await ExecuteReaderAsync(command, reader =>
        {
            pk ??= new PrimaryKey { Name = reader.GetString(0) };

            pk.ColumnNames.Add(new OrderColumn
            {
                Name = reader.GetString(1),
                Direction = reader.GetBoolean(2) ? Direction.Descending : Direction.Ascending
            });
        }, ct).ConfigureAwait(false);
        _logger.Information(
            "Retrieved primary key {@PrimaryKey} for '{tableName}'",
            pk, tableHeader.Name);
        return pk;
    }

    public async Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var foreignKeys = await GetForeignKeyReferencesAsync(tableHeader, ct).ConfigureAwait(false);

        return foreignKeys;
    }

    public async Task<IEnumerable<IndexDefinition>> GetIndexesAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var command =
            new SqlCommand("select i.index_id, i.object_id, i.name, i.type, i.is_unique, f.name\r\n" +
                           "from sys.indexes i\r\n" +
                           "join sys.filegroups f on (i.data_space_id = f.data_space_id)\r\n" +
                           "where i.is_primary_key = 0\r\n" +
                           "and i.index_id != 0\r\n" +
                           "and i.object_id = @TableId");
        command.Parameters.AddWithValue("@TableId", tableHeader.Id);

        var indexes = new List<IndexDefinition>();
        await ExecuteReaderAsync(command, async reader =>
        {
            try
            {
                var index = new IndexDefinition
                {
                    Header = new IndexHeader
                    {
                        Id = reader.GetInt32(0),
                        TableId = reader.GetInt32(1),
                        Name = reader.IsDBNull(2) ? "Heap" : reader.GetString(2),
                        Type = (IndexType)reader.GetByte(3),
                        IsUnique = reader.GetBoolean(4),
                        Location = reader.IsDBNull(5) ? "PRIMARY" : reader.GetString(5)
                    }
                };

                var indexColumns = await GetIndexColumnInfoAsync(tableHeader.Name, index.Header, ct).ConfigureAwait(false);
                index.Columns.AddRange(indexColumns);
                indexes.Add(index);

                _logger.Verbose("Added index: {IndexName}", index.Header.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }, ct).ConfigureAwait(false);

        _logger.Information(
            $"Retrieved {{IndexCount}} {"index".Plural(indexes.Count)} for table '{{tableName}}'",
            indexes.Capacity, tableHeader.Name);

        return indexes;
    }

    private async Task<List<ForeignKey>> GetForeignKeyReferencesAsync(
        TableHeader tableHeader, CancellationToken ct)
    {
        var command =
            new SqlCommand("SELECT DISTINCT  \r\n" +
                           "    f.name AS foreign_key_name  \r\n" +
                           "   ,object_id \r\n" +
                           "   ,OBJECT_NAME (f.referenced_object_id) AS referenced_object  \r\n" +
                           "   ,f.delete_referential_action_desc  \r\n" +
                           "   ,f.update_referential_action_desc  \r\n" +
                           "FROM sys.foreign_keys AS f  \r\n" +
                           "WHERE f.parent_object_id = @TableId");
        command.Parameters.AddWithValue("@TableId", tableHeader.Id);

        var foreignKeys = new List<ForeignKey>();
        await ExecuteReaderAsync(command, async reader =>
        {
            var fk = new ForeignKey
            {
                ConstraintName = reader.GetString(0),
                ConstraintId = reader.GetInt32(1),
                TableReference = reader.GetString(2),
                DeleteAction = (DeleteAction)Enum.Parse(typeof(DeleteAction), reader.GetString(3).Replace("_", ""), true),
                UpdateAction = (UpdateAction)Enum.Parse(typeof(UpdateAction), reader.GetString(4).Replace("_", ""), true)
            };
            await GetForeignKeyColumnsAsync(fk, ct).ConfigureAwait(false);
            foreignKeys.Add(fk);
            _logger.Information("Found foreign key: {ForeignKeyReference} on table {TableName}",
                fk.ConstraintName, tableHeader.Name);
        }, ct).ConfigureAwait(false);

        return foreignKeys;
    }

    private async Task GetForeignKeyColumnsAsync(
        ForeignKey foreignKey, CancellationToken ct)
    {
        var command =
            new SqlCommand("SELECT   \r\n" +
                           "    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS constraint_column_name,  \r\n" +
                           "    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name  \r\n" +
                           "FROM sys.foreign_key_columns AS fc   \r\n" +
                           "WHERE fc.constraint_object_id = @ConstraintId\r\n" +
                           "ORDER BY fc.constraint_column_id");
        command.Parameters.AddWithValue("@ConstraintId", foreignKey.ConstraintId);

        await ExecuteReaderAsync(command, reader =>
        {
            foreignKey.ColumnNames.Add(reader.GetString(0));
            foreignKey.ColumnReferences.Add(reader.GetString(1));
        }, ct).ConfigureAwait(false);

        _logger.Debug(
            "Added {KeyColumnCount} columns to constraint '{ConstraintName}'",
            foreignKey.ColumnNames.Count,
            foreignKey.ConstraintName);
    }

    private async Task<IEnumerable<IndexColumn>> GetIndexColumnInfoAsync(
        string tableName, IndexHeader indexHeader, CancellationToken ct)
    {
        var command =
            new SqlCommand("select COL_NAME(ic.object_id,ic.column_id) AS column_name, ic.is_descending_key\r\n" +
                           "from sys.indexes i\r\n" +
                           "join sys.index_columns ic on (ic.object_id = i.object_id and ic.index_id = i.index_id)\r\n" +
                           "where i.index_id = @IndexId\r\n" +
                           "and i.is_primary_key = 0\r\n" +
                           "and i.object_id = object_id(@TableName)");

        command.Parameters.AddWithValue("@IndexId", indexHeader.Id);
        command.Parameters.AddWithValue("@TableName", tableName);

        var columns = new List<IndexColumn>();
        await ExecuteReaderAsync(command, reader =>
        {
            var column = new IndexColumn
            {
                Name = reader.GetString(0),
                Direction = reader.GetBoolean(1) ? Direction.Descending : Direction.Ascending
            };
            columns.Add(column);
            _logger.Verbose("Added column: {ColumnName}", column.Name);
        }, ct).ConfigureAwait(false);

        _logger.Information(
            $"Retrieved {{ColumnCount}} index {"column".Plural(columns.Count)} for index '{{TableName}}.{{IndexName}}'",
            columns.Capacity, tableName, indexHeader.Name);

        return columns;
    }
}