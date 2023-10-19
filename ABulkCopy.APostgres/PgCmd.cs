namespace ABulkCopy.APostgres;

public class PgCmd : IPgCmd
{
    private readonly IPgContext _pgContext;
    private readonly IQueryBuilderFactory _queryBuilderFactory;
    private readonly IPgSystemTables _systemTables;
    private readonly ILogger _logger;

    public PgCmd(
        IPgContext pgContext,
        IQueryBuilderFactory queryBuilderFactory,
        IPgSystemTables systemTables,
        ILogger logger)
    {
        _pgContext = pgContext;
        _queryBuilderFactory = queryBuilderFactory;
        _systemTables = systemTables;
        _logger = logger.ForContext<PgCmd>();
    }

    public async Task DropTable(string tableName)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQuery(qb.CreateDropTableStmt(tableName));
    }

    public async Task CreateTable(TableDefinition tableDefinition, bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        qb.Append("create table ");
        if (addIfNotExists) qb.Append("if not exists ");
        qb.AppendIdentifier(tableDefinition.Header.Name);
        qb.AppendLine(" (");
        qb.AppendColumns(tableDefinition);
        AddPrimaryKeyClause(tableDefinition, qb);
        AddForeignKeyClauses(tableDefinition, qb);
        qb.AppendLine(");");
        await ExecuteNonQuery(qb.ToString());
    }

    public async Task CreateIndex(string tableName, IndexDefinition indexDefinition)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        qb.Append("create ");
        if (indexDefinition.Header.IsUnique) qb.Append("unique ");
        qb.Append("index ");
        qb.AppendIdentifier(indexDefinition.Header.Name);
        qb.AppendLine(" ");
        qb.Append("on ");
        qb.AppendIdentifier(tableName);
        AddIndexColumns(qb, indexDefinition.Columns, true);
        if (indexDefinition.Columns.Any(c => c.IsIncluded))
        {
            qb.AppendLine(" include ");
            AddIndexColumns(qb, indexDefinition.Columns);
        }

        await ExecuteNonQuery(qb.ToString());
    }

    public async Task ResetIdentity(string tableName, string columnName)
    {
        var oid = await _systemTables.GetIdentityOid(tableName, columnName);
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

        await using var cmd = _pgContext.DataSource.CreateCommand(qb.ToString());
        await cmd.ExecuteScalarAsync();
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<object?> SelectScalar(string sqlString)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        return await cmd.ExecuteScalarAsync();
    }

    private void AddForeignKeyClauses(TableDefinition tableDefinition, IQueryBuilder qb)
    {
        if (!tableDefinition.ForeignKeys.Any())
            return;


        foreach (var fk in tableDefinition.ForeignKeys)
        {
            qb.AppendLine(", ");
            qb.Append("    ");
            AddConstraintName(fk.ConstraintName, qb);
            qb.Append(" foreign key (");

            qb.AppendIdentifierList(fk.ColumnNames);

            qb.Append(") references ");
            qb.AppendIdentifier(fk.TableReference);
            qb.Append(" (");
            qb.AppendIdentifierList(fk.ColumnReferences);
            qb.Append(") ");

            qb.Append(fk.UpdateAction.GetClause());
            qb.Append(fk.DeleteAction.GetClause());
        }
    }

    private void AddConstraintName(string? name, IQueryBuilder qb)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (name.Length > _pgContext.MaxIdentifierLength)
        {
            _logger.Warning("Constraint name '{Name}' is too long. Max length is {MaxIdentifierLength} characters. Constraint name is not used.",
                name, _pgContext.MaxIdentifierLength);
            return;
        }

        qb.Append("constraint ");
        qb.AppendIdentifier(name);
    }

    private void AddPrimaryKeyClause(TableDefinition tableDefinition, IQueryBuilder qb)
    {
        if (tableDefinition.PrimaryKey == null)
            return;

        qb.AppendLine(",");
        qb.Append("    constraint ");
        qb.AppendIdentifier(tableDefinition.PrimaryKey.Name);
        qb.Append(" primary key (");
        qb.AppendIdentifierList(tableDefinition.PrimaryKey.ColumnNames.Select(c => c.Name));
        qb.Append(") ");
    }

    private static void AddIndexColumns(
        IQueryBuilder qb,
        IEnumerable<IndexColumn> columns,
        bool addDirection = false)
    {
        qb.AppendLine(" (");
        var first = true;
        foreach (var column in columns.Where(c => !c.IsIncluded))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                qb.AppendLine(",");
            }

            qb.Append("    ");
            qb.AppendIdentifier(column.Name);
            qb.Append(" ");
            if (addDirection && column.Direction == Direction.Descending) qb.Append("desc ");
        }

        qb.AppendLine(")");
    }
}