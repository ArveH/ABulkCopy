namespace ABulkCopy.ASqlServer.DbRaw;

public class MssRawFactory : IMssRawFactory
{
    public IDbRawReader CreateReader(DbDataReader reader)
    {
        return new MssRawReader((SqlDataReader)reader);
    }
}