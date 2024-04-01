namespace End2EndTests;

public class TestCopying : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = 
        new MsSqlBuilder().Build();

    [Fact]
    public async Task TestCopyOutFromMss()
    {
        const string tableName = "AllTypes";
        var connectionString = _msSqlContainer.GetConnectionString();
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

        var args = cmdArguments!.ToAppSettings();

        await using var connection = new SqlConnection(_msSqlContainer.GetConnectionString());
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1;";

        var actual = await command.ExecuteScalarAsync() as int?;
        Assert.Equal(1, actual.GetValueOrDefault());

        var configuration = new ConfigHelper().GetConfiguration(
            null, cmdArguments.ToAppSettings());

        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices(Rdbms.Mss, configuration);
        IServiceProvider provider = services.BuildServiceProvider();

        var copyOut = provider.GetRequiredService<ICopyOut>();
        await copyOut.RunAsync(CancellationToken.None);


        var schemaFile = await File.ReadAllTextAsync(tableName + ".schema");
        schemaFile.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFile.Should().Contain("\"Name\": \"AllTypes\"");
        var dataFile = await File.ReadAllTextAsync(tableName + ".data");
        dataFile.Should().NotBeNull("because the data file should exist");
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

    public Task InitializeAsync()
        => _msSqlContainer.StartAsync();

    public Task DisposeAsync()
        => _msSqlContainer.DisposeAsync().AsTask();
}