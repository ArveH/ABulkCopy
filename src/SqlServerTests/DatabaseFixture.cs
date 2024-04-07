namespace SqlServerTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _mssContainer =
        new MsSqlBuilder().Build();

    public string MssConnectionString => _mssContainer.GetConnectionString();
    public string MssContainerId => $"{_mssContainer.Id}";

    public async Task InitializeAsync()
    {
        await _mssContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mssContainer.DisposeAsync();
    }
}