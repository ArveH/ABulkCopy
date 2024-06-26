﻿namespace ABulkCopy.APostgres;

public class QueryBuilderFactory : IQueryBuilderFactory
{
    private readonly IIdentifier _identifier;

    public QueryBuilderFactory(IIdentifier identifier)
    {
        _identifier = identifier;
    }

    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder(_identifier);
    }
}