namespace ABulkCopy.APostgres;

public class PgCmd : PgCommandBase, IPgCmd
{
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public PgCmd(
        IPgContext pgContext,
        IQueryBuilderFactory queryBuilderFactory) : base(pgContext)
    {
        _queryBuilderFactory = queryBuilderFactory;
    }

    public async Task DropTableAsync(SchemaTableTuple st, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(qb.CreateDropTableStmt(st), ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(TableDefinition tableDefinition, CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        qb.Append("create table ");
        if (addIfNotExists) qb.Append("if not exists ");
        qb.AppendIdentifier(tableDefinition.Header.Schema);
        qb.Append(".");
        qb.AppendIdentifier(tableDefinition.Header.Name);
        qb.AppendLine(" (");
        qb.AppendColumns(tableDefinition);
        AddPrimaryKeyClause(tableDefinition, qb);
        AddForeignKeyClauses(tableDefinition, qb);
        qb.AppendLine(");");
        await ExecuteNonQueryAsync(qb.ToString(), ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(
        string tableName, IndexDefinition indexDefinition, CancellationToken ct)
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

        await ExecuteNonQueryAsync(qb.ToString(), ct).ConfigureAwait(false);
    }

    private void AddForeignKeyClauses(TableDefinition tableDefinition, IQueryBuilder qb)
    {
        if (!tableDefinition.ForeignKeys.Any())
            return;


        foreach (var fk in tableDefinition.ForeignKeys)
        {
            qb.AppendLine(", ");
            qb.Append("    ");
            qb.Append(" foreign key (");

            qb.AppendIdentifierList(fk.ColumnNames);

            qb.Append(") references ");
            qb.AppendIdentifier(fk.SchemaReference);
            qb.Append(".");
            qb.AppendIdentifier(fk.TableReference);
            qb.Append(" (");
            qb.AppendIdentifierList(fk.ColumnReferences);
            qb.Append(") ");

            qb.Append(fk.UpdateAction.GetClause());
            qb.Append(fk.DeleteAction.GetClause());
        }
    }

    private void AddPrimaryKeyClause(TableDefinition tableDefinition, IQueryBuilder qb)
    {
        if (tableDefinition.PrimaryKey == null)
            return;

        qb.AppendLine(",");
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