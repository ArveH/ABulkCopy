namespace Testing.Shared.Postgres;

public class PgDbHelper
{
    private readonly IPgCmd _pgCmd;

    public const string TestSchemaName = "test_schema";

    public PgDbHelper(IPgCmd pgCmd)
    {
        _pgCmd = pgCmd;
    }

    public async Task EnsureTestSchemaAsync()
    {
        await ExecuteNonQueryAsync($"CREATE SCHEMA IF NOT EXISTS {TestSchemaName}");
    }

    private async Task ExecuteNonQueryAsync(string sql)
    {
        await _pgCmd.ExecuteNonQueryAsync(sql, CancellationToken.None);
    }

    private async Task<T?> ExecuteScalarAsync<T>(string sql)
    {
        return (T?)await _pgCmd.SelectScalarAsync(sql, CancellationToken.None);
    }
}