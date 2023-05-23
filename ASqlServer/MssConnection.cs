namespace ASqlServer;

public class MssConnection : IDbContext
{
    public MssConnection()
    {
        DbServer = DbServer.SqlServer;
    }

    public required string ConnectionString { get; init; }
    public DbServer DbServer { get; }
}