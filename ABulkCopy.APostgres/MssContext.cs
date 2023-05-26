namespace ABulkCopy.APostgres;

public class PgContext : IDbContext
{
    public PgContext()
    {
        DbServer = DbServer.Postgres;
    }

    public required string ConnectionString { get; init; }
    public DbServer DbServer { get; }
}