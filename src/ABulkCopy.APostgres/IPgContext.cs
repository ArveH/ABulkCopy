namespace ABulkCopy.APostgres;

public interface IPgContext : IDbContext
{
    NpgsqlDataSource DataSource { get; }
}