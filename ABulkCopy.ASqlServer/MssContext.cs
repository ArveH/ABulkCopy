namespace ABulkCopy.ASqlServer;

public class MssContext : IDbContext
{
    public MssContext()
    {
        DbServer = DbServer.SqlServer;
    }

    public required string ConnectionString { get; init; }
    public DbServer DbServer { get; }
}