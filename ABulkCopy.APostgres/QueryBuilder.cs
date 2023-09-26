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

    public string CreateDropTableStmt(string tableName)
    {
        _sb.Clear();
        _sb.Append("drop table if exists ");
        AppendIdentifier(tableName);
        _sb.Append(";");
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

    public void AppendColumnNames(TableDefinition tableDefinition)
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
            _sb.Append(column.GetNativeCreateClause());
        }
    }
}