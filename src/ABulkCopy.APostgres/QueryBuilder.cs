namespace ABulkCopy.APostgres;

public class QueryBuilder : IQueryBuilder
{
    private readonly IIdentifier _identifier;
    private readonly StringBuilder _sb = new();

    public QueryBuilder(IIdentifier identifier)
    {
        _identifier = identifier;
    }

    public void AppendIdentifier(string identifier)
    {
        _sb.Append(_identifier.Get(identifier));
    }

    public void Append(string str) => _sb.Append(str);
    public void AppendLine(string str) => _sb.AppendLine(str);
    public override string ToString() => _sb.ToString();

    public string CreateDropTableStmt(SchemaTableTuple st)
    {
        _sb.Clear();
        _sb.Append("drop table if exists ");
        AppendIdentifier(st.schemaName);
        _sb.Append(".");
        AppendIdentifier(st.tableName);
        _sb.Append(';');
        return _sb.ToString();
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
}