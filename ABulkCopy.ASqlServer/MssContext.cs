namespace ABulkCopy.ASqlServer;

public class MssContext : IDbContext
{
    public MssContext()
    {
        Rdbms = Rdbms.SqlServer;
    }

    public required string ConnectionString { get; init; }
    public Rdbms Rdbms { get; }
}