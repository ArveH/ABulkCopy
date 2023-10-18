namespace ABulkCopy.Cmd.Factories;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    private readonly IPgParser _pgParser;
    private readonly IIdentifier _identifier;

    public QueryBuilderFactory(
        IPgParser pgParser,
        IIdentifier identifier)
    {
        _pgParser = pgParser;
        _identifier = identifier;
    }

    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder(_pgParser, _identifier);
    }
}