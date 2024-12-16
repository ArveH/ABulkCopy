namespace ABulkCopy.APostgres.DbRaw;

public class DbRawFactory : IDbRawFactory
{
    public IDbRawReader CreateReader(DbDataReader reader)
    {
        return new PgRawReader(reader);
    }
}