namespace SqlServer.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer? _mssContainer;
    private IDbContext? _mssDbContext;
    private IMssRawCommand? _mssRawCommand;
    private IMssCmd? _mssCmd;
    private IConfiguration? _testConfiguration;
    private string? _connectionString;

    public string MssConnectionString => _connectionString ?? throw new ArgumentNullException(nameof(MssConnectionString));
    public const string TestSchemaName = "my_mss_schema";
    
    public IDbContext MssDbContext
    {
        get => _mssDbContext ?? throw new ArgumentNullException(nameof(MssDbContext));
        set => _mssDbContext = value;
    }
    
    public IMssRawCommand MssRawCommand
    {
        get => _mssRawCommand ?? throw new ArgumentNullException(nameof(MssRawCommand));
        private set => _mssRawCommand = value;
    }

    public IMssCmd MssCmd
    {
        get => _mssCmd ?? throw new ArgumentNullException(nameof(MssCmd));
        private set => _mssCmd = value;
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
            _mssContainer = new MsSqlBuilder().Build();
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
        _mssRawCommand = new MssRawCommand(MssDbContext, new MssRawFactory());
        _mssCmd = new MssCmd(
            _mssRawCommand, 
            new QueryBuilderFactory(), 
            new LoggerConfiguration().WriteTo.Console().CreateLogger());
        
        await MssCmd.EnsureSchemaAsync(TestSchemaName);
    }

    public async Task DisposeAsync()
    {
        if (_mssContainer != null)
        {
            await _mssContainer.DisposeAsync();
        }
    }
}