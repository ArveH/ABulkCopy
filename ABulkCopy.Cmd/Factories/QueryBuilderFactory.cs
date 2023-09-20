namespace ABulkCopy.Cmd.Factories;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    private readonly bool _addQuotes;

    public QueryBuilderFactory(IConfiguration config)
    {
        _addQuotes = Convert.ToBoolean(config[Constants.Config.AddQuotes]);
    }

    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder(_addQuotes);
    }
}