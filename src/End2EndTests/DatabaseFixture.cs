using Testcontainers.PostgreSql;

namespace End2EndTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _mssContainer =
        new MsSqlBuilder().Build();
    private readonly PostgreSqlContainer _pgContainer =
        new PostgreSqlBuilder().Build();

    public string MssConnectionString => _mssContainer.GetConnectionString();
    public string PgConnectionString => _pgContainer.GetConnectionString();
    public string MssContainerId => $"{_mssContainer.Id}";
    public string PgContainerId => $"{_pgContainer.Id}";

    public async Task InitializeAsync()
    {
        await _mssContainer.StartAsync();
        await _pgContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mssContainer.DisposeAsync();
        await _pgContainer.DisposeAsync();
    }
}