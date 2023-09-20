namespace APostgres.Test;

public class QBFactoryForTest : IQueryBuilderFactory
{
    public bool AddQuotes { get; set; } = true;

    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder(AddQuotes);
    }
}