namespace ABulkCopy.APostgres;

public class QueryBuilder : IQueryBuilder
{
    private readonly IPgParser _pgParser;
    private readonly IIdentifier _identifier;
    private readonly StringBuilder _sb = new();

    public QueryBuilder(
        IPgParser pgParser,
        IIdentifier identifier)
    {
        _pgParser = pgParser;
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
            _sb.Append(column.GetTypeClause());
            _sb.Append(column.GetIdentityClause());
            if (column.HasDefault)
            {
                // TODO: Inject factories or otherwise remove all 'new' statements
                ITokenizer tokenizer = new Tokenizer(new TokenFactory());
                tokenizer.Initialize(column.DefaultConstraint!.Definition);
                tokenizer.GetNext();
                IParseTree parseTree = new ParseTree(new AParser.Tree.NodeFactory(), new SqlTypes());
                var root = parseTree.CreateExpression(tokenizer);
                _sb.Append(" default ");
                _sb.Append(_pgParser.Parse(tokenizer, root));
            }
            _sb.Append(column.GetNullableClause());
        }
    }
}