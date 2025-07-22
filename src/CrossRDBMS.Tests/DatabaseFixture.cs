using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using ABulkCopy.ASqlServer.DbRaw;
using QueryBuilderFactory = ABulkCopy.ASqlServer.QueryBuilderFactory;

namespace CrossRDBMS.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _pgContainer;
    private IPgContext? _pgContext;
    
    private MsSqlContainer? _mssContainer;
    private IMssRawCommand? _mssRawCommand;
    private IMssCmd? _mssCmd;
    private IDbContext? _mssContext;

    private IConfiguration? _testConfiguration;
    private string? _pgConnectionString;
    private string? _mssConnectionString;
    
    public const string PgTestSchemaName = "my_pg_schema";
    public const string MssTestSchemaName = "my_mss_schema";
    
    public string MssConnectionString =>
        _mssConnectionString ?? throw new ArgumentNullException(nameof(MssConnectionString));

    public string PgConnectionString =>
        _pgConnectionString ?? throw new ArgumentNullException(nameof(PgConnectionString));

    public IConfiguration TestConfiguration
    {
        get => _testConfiguration ?? throw new ArgumentNullException(nameof(TestConfiguration));
        set => _testConfiguration = value;
    }

    public IDbContext MssContext
    {
        get => _mssContext ?? throw new ArgumentNullException(nameof(MssContext));
        set => _mssContext = value;
    }

    public IMssRawCommand MssRawCommand
    {
        get => _mssRawCommand ?? throw new ArgumentNullException(nameof(MssRawCommand));
        set => _mssRawCommand = value;
    }
    
    public IMssCmd MssCmd
    {
        get => _mssCmd ?? throw new ArgumentNullException(nameof(MssCmd));
        set => _mssCmd = value;
    }
    
    public IPgContext PgContext
    {
        get => _pgContext ?? throw new ArgumentNullException(nameof(PgContext));
        private set => _pgContext = value;
    }

    public async Task InitializeAsync()
    {
        TestConfiguration = new ConfigHelper().GetConfiguration(
            "4c4ed632-229d-49da-88b9-454aa5a4a83c");
        if (TestConfiguration.UseContainer())
        {
            _mssContainer =
                new MsSqlBuilder()
                    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                    .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                    .WithPortBinding(1433, true)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                    .Build();
            _pgContainer =
                new PostgreSqlBuilder()
                    .WithPortBinding(54770, 5432)
                    .Build();
            await _mssContainer.StartAsync();
            await _pgContainer.StartAsync();
            TestConfiguration = new ConfigHelper().GetConfiguration(
                "4c4ed632-229d-49da-88b9-454aa5a4a83c",
                new()
                {
                    {
                        "ConnectionStrings:" + Constants.Config.MssConnectionString, _mssContainer.GetConnectionString()
                    },
                    {
                        "ConnectionStrings:" + Constants.Config.PgConnectionString, _pgContainer.GetConnectionString()
                    }
                });
        }
        _pgConnectionString = TestConfiguration.GetConnectionString(Constants.Config.PgConnectionString);
        PgContext = new PgContext(new NullLoggerFactory(), TestConfiguration);
        var pgCmd = new PgCmd(
            new PgRawCommand(PgContext, new PgRawFactory()),
            new QueryBuilderFactory());
        await pgCmd.EnsureSchemaAsync(PgTestSchemaName);

        _mssConnectionString = TestConfiguration.GetConnectionString(Constants.Config.MssConnectionString);
        MssContext = new MssContext(TestConfiguration);
        _mssRawCommand = new MssRawCommand(
            MssContext,
            new MssRawFactory());
        _mssCmd = new MssCmd(
            _mssRawCommand,
            new QueryBuilderFactory(),
            new LoggerConfiguration().WriteTo.Console().CreateLogger());
        await _mssCmd.EnsureSchemaAsync(MssTestSchemaName);
    }

    public async Task DisposeAsync()
    {
        if (_mssContainer != null)
        {
            await _mssContainer.DisposeAsync();
        }

        if (_pgContainer != null)
        {
            await _pgContainer.DisposeAsync();
        }
        _pgContext?.Dispose();
    }
}