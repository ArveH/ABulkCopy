namespace ABulkCopy.ASqlServer;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder();
    }
}