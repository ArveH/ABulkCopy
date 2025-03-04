using DotNet.Testcontainers.Builders;

namespace SqlServer.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer? _mssContainer;
    private IDbContext? _mssDbContext;
    private MssDbHelper? _mssDbHelper;
    private IConfiguration? _testConfiguration;
    private string? _connectionString;

    public string MssConnectionString => _connectionString ?? throw new ArgumentNullException(nameof(MssConnectionString));

    public IDbContext MssDbContext
    {
        get => _mssDbContext ?? throw new ArgumentNullException(nameof(MssDbContext));
        set => _mssDbContext = value;
    }

    public MssDbHelper DbHelper
    {
        get => _mssDbHelper ?? throw new ArgumentNullException(nameof(MssDbHelper));
        set => _mssDbHelper = value;
    }

    public IConfiguration TestConfiguration
    {
        get => _testConfiguration ?? throw new ArgumentNullException(nameof(TestConfiguration));
        set => _testConfiguration = value;
    }

    public async Task InitializeAsync()
    {
        TestConfiguration = new ConfigHelper().GetConfiguration(
            "5a78c96d-6df9-4362-ba25-4afceae69c52");
        if (TestConfiguration.UseContainer())
        {
            _mssContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(1433, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
            await _mssContainer.StartAsync();
            TestConfiguration = new ConfigHelper().GetConfiguration(
                    "5a78c96d-6df9-4362-ba25-4afceae69c52", 
                    new()
                    {
                        {"ConnectionStrings:" + Constants.Config.MssConnectionString, 
                            _mssContainer.GetConnectionString()}
                    });
        }
        _connectionString = TestConfiguration.GetConnectionString(Constants.Config.MssConnectionString);

        MssDbContext = new MssContext(TestConfiguration);
        _mssDbHelper = new MssDbHelper(MssDbContext, new QueryBuilderFactory());
        await DbHelper.EnsureTestSchemaAsync();
    }

    public async Task DisposeAsync()
    {
        if (_mssContainer != null)
        {
            await _mssContainer.DisposeAsync();
        }
    }
}