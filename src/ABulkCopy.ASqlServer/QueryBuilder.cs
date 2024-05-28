namespace ABulkCopy.ASqlServer;

public class QueryBuilder : IQueryBuilder
{
    private readonly StringBuilder _sb = new();

    public void AppendIdentifier(string identifier)
    {
        _sb.Append("[");
        _sb.Append(identifier);
        _sb.Append("]");
    }

    public void Append(string str) => _sb.Append(str);
    public void AppendLine(string str) => _sb.AppendLine(str);
    public override string ToString() => _sb.ToString();

    public string CreateTableStmt(TableDefinition tableDefinition, bool addIfNotExists = false)
    {
        if (addIfNotExists)
        {
            AppendLine("if not exists ( ");
            AppendLine("    select 1 from sys.tables t ");
            AppendLine("        join sys.schemas s on (t.schema_id = s.schema_id) ");
            AppendLine($"    where s.name = '{tableDefinition.Header.Schema}' and t.name = '{tableDefinition.Header.Name}')");
            AppendLine("begin");
        }

        Append("create table ");
        AppendIdentifier(tableDefinition.Header.Schema);
        Append(".");
        AppendIdentifier(tableDefinition.Header.Name);
        AppendLine(" (");
        AppendColumns(tableDefinition);
        AddPrimaryKeyClause(tableDefinition);
        AddForeignKeyClauses(tableDefinition);
        AppendLine("); ");

        if (addIfNotExists)
        {
            AppendLine("end");
        }

        return _sb.ToString();
    }

    public string DropTableStmt(SchemaTableTuple st)
    {
        _sb.Clear();
        _sb.Append("drop table if exists ");
        AppendIdentifier(st.schemaName);
        _sb.Append(".");
        AppendIdentifier(st.tableName);
        _sb.Append(';');
        return _sb.ToString();
    }

    public string CreateIndexStmt((string schemaName, string tableName) st, IndexDefinition indexDefinition)
    {
        throw new NotImplementedException();
    }

    public void AppendIdentifierList(IEnumerable<string> names)
    {
        var first = true;
        foreach (var name in names)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                _sb.Append(", ");
            }

            AppendIdentifier(name);
        }
    }

    public void AppendColumns(TableDefinition tableDefinition)
    {
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                _sb.AppendLine(",");
            }

            _sb.Append("     ");
            AppendIdentifier(column.Name);
            _sb.Append(" ");
            _sb.Append(column.GetTypeClause());
            _sb.Append(column.GetIdentityClause());
            if (column.HasDefault)
            {
                _sb.Append(" default ");
                _sb.Append(column.DefaultConstraint!.Definition);
            }
            _sb.Append(column.GetNullableClause());
        }
    }

    private void AddForeignKeyClauses(TableDefinition tableDefinition)
    {
        if (!tableDefinition.ForeignKeys.Any())
            return;


        foreach (var fk in tableDefinition.ForeignKeys)
        {
            AppendLine(", ");
            Append("    ");
            Append(" foreign key (");

            AppendIdentifierList(fk.ColumnNames);

            Append(") references ");
            AppendIdentifier(fk.SchemaReference);
            Append(".");
            AppendIdentifier(fk.TableReference);
            Append(" (");
            AppendIdentifierList(fk.ColumnReferences);
            Append(") ");

            Append(fk.UpdateAction.GetClause());
            Append(fk.DeleteAction.GetClause());
        }
    }

    private void AddPrimaryKeyClause(TableDefinition tableDefinition)
    {
        if (tableDefinition.PrimaryKey == null)
            return;

        AppendLine(",");
        Append(" primary key (");
        AppendIdentifierList(tableDefinition.PrimaryKey.ColumnNames.Select(c => c.Name));
        Append(") ");
    }
}