using Testcontainers.PostgreSql;

namespace End2EndTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _mssContainer =
        new MsSqlBuilder().Build();
    private readonly PostgreSqlContainer _pgContainer =
        new PostgreSqlBuilder()
            .WithPortBinding(54770, 5432)
            .Build();
    private NpgsqlDataSource? _pgDataSource;

    public string MssConnectionString => _mssContainer.GetConnectionString();
    public string PgConnectionString => _pgContainer.GetConnectionString();
    public string MssContainerId => $"{_mssContainer.Id}";
    public string PgContainerId => $"{_pgContainer.Id}";
    public NpgsqlDataSource PgDataSource => _pgDataSource ?? throw new ArgumentNullException(nameof(PgDataSource));

    public async Task InitializeAsync()
    {
        await _mssContainer.StartAsync();
        await _pgContainer.StartAsync();

        _pgDataSource = new NpgsqlDataSourceBuilder(PgConnectionString).Build();
    }

    public async Task DisposeAsync()
    {
        await _mssContainer.DisposeAsync();
        await _pgContainer.DisposeAsync();
        if (_pgDataSource != null)
        {
            await _pgDataSource.DisposeAsync();
        }
    }
}