namespace ABulkCopy.APostgres;

public interface IPgContext : IDbContext, IDisposable
{
    NpgsqlDataSource DataSource { get; }
}