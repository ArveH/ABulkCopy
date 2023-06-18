namespace ABulkCopy.ASqlServer;

public class MssContext : IDbContext
{
    public MssContext()
    {
        Rdbms = Rdbms.Mss;
    }

    public required string ConnectionString { get; init; }
    public Rdbms Rdbms { get; }
}