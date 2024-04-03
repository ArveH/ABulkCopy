namespace End2EndTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer =
        new MsSqlBuilder().Build();

    public string ConnectionString => _msSqlContainer.GetConnectionString();
    public string ContainerId => $"{_msSqlContainer.Id}";

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _msSqlContainer.DisposeAsync().AsTask();
    }
}