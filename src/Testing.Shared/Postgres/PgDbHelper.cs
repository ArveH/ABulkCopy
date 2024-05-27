namespace Testing.Shared.Postgres;

public class PgDbHelper : PgCommandBase
{
    public const string TestSchemaName = "my_pg_schema";

    public PgDbHelper(IPgContext pgContext) : base(pgContext)
    {
    }

    public async Task EnsureTestSchemaAsync()
    {
        await ExecuteNonQueryAsync($"CREATE SCHEMA IF NOT EXISTS {TestSchemaName}");
    }

    private async Task ExecuteNonQueryAsync(string sql)
    {
        await ExecuteNonQueryAsync(sql, CancellationToken.None);
    }

    private async Task<T?> ExecuteScalarAsync<T>(string sql)
    {
        return (T?)await SelectScalarAsync(sql, CancellationToken.None);
    }
}