namespace ABulkCopy.APostgres.DbRaw;

public interface IDbRawFactory
{
    IDbRawReader CreateReader(DbDataReader reader);
}