namespace End2EndTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer =
        new MsSqlBuilder().Build();

    private IServiceProvider? _mssServiceProvider;

    public string ConnectionString => _msSqlContainer.GetConnectionString();
    public string ContainerId => $"{_msSqlContainer.Id}";

    public IServiceProvider MssServiceProvider => _mssServiceProvider ??
                                                  throw new InvalidOperationException("Service provider is null");

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        const string tableName = "AllTypes";
        var connectionString = ConnectionString;
        var cmdArguments = GetArguments([
            "-d",
            "out",
            "-r",
            "Mss",
            "-c",
            connectionString,
            "-l",
            ".\\local_out.log",
            "-f",
            ".\\",
            "-s",
            tableName
        ]);
        cmdArguments.Should().NotBeNull("because there are command line arguments");

        var configuration = new ConfigHelper().GetConfiguration(
            null, cmdArguments!.ToAppSettings());

        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices(Rdbms.Mss, configuration);
        _mssServiceProvider = services.BuildServiceProvider();
    }

    public Task DisposeAsync()
    {
        return _msSqlContainer.DisposeAsync().AsTask();
    }

    private static CmdArguments? GetArguments(string[] args)
    {
        var parser = new Parser(cfg =>
        {
            cfg.CaseInsensitiveEnumValues = true;
            cfg.HelpWriter = Console.Error;
        });
        var result = parser.ParseArguments<CmdArguments>(args);
        if (result.Tag == ParserResultType.NotParsed)
        {
            // A usage message is written to Console.Error by the CommandLineParser
            return null;
        }

        return result.Value;
    }
}