using Microsoft.Extensions.Logging.Abstractions;
using QueryBuilderFactory = ABulkCopy.ASqlServer.QueryBuilderFactory;

namespace CrossRDBMS.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer? _mssContainer;
    private PostgreSqlContainer? _pgContainer;
    private IDbContext? _mssContext;
    private MssDbHelper? _mssDbHelper;
    private IPgContext? _pgContext;
    private PgDbHelper? _pgDbHelper;
    private IConfiguration? _testConfiguration;
    private string? _pgConnectionString;
    private string? _mssConnectionString;

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

    public MssDbHelper MssDbHelper
    {
        get => _mssDbHelper ?? throw new ArgumentNullException(nameof(MssDbHelper));
        set => _mssDbHelper = value;
    }

    public IPgContext PgContext
    {
        get => _pgContext ?? throw new ArgumentNullException(nameof(PgContext));
        private set => _pgContext = value;
    }

    public PgDbHelper PgDbHelper
    {
        get => _pgDbHelper ?? throw new ArgumentNullException(nameof(PgDbHelper));
        set => _pgDbHelper = value;
    }

    public async Task InitializeAsync()
    {
        TestConfiguration = new ConfigHelper().GetConfiguration(
            "4c4ed632-229d-49da-88b9-454aa5a4a83c");
        if (TestConfiguration.UseContainer())
        {
            _mssContainer =
                new MsSqlBuilder().Build();
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
        _mssConnectionString = TestConfiguration.GetConnectionString(Constants.Config.MssConnectionString);
        _pgConnectionString = TestConfiguration.GetConnectionString(Constants.Config.PgConnectionString);
        PgContext = new PgContext(new NullLoggerFactory(), TestConfiguration);
        _pgDbHelper = new PgDbHelper(PgContext);
        MssContext = new MssContext(TestConfiguration);
        _mssDbHelper = new MssDbHelper(MssContext, new QueryBuilderFactory());
        //await _pgDbHelper.EnsureTestSchemaAsync();
        //await _mssDbHelper.EnsureTestSchemaAsync();
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