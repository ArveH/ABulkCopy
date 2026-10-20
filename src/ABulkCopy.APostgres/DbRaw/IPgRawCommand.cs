namespace ABulkCopy.APostgres.DbRaw;

public interface IPgRawCommand : IDbRawCommand
{
    NpgsqlDataSource DataSource { get; }
}