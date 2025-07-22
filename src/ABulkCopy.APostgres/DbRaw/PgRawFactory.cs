namespace ABulkCopy.APostgres.DbRaw;

public class PgRawFactory : IPgRawFactory
{
    public IDbRawReader CreateReader(DbDataReader reader)
    {
        return new PgRawReader(reader);
    }
}