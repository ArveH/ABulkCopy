namespace ABulkCopy.Common.Database;

public interface IDbRawFactory
{
    IDbRawReader CreateReader(DbDataReader reader);
}