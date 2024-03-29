using ABulkCopy.Cmd;
using ABulkCopy.Common.Config;
using ABulkCopy.Common.Types;
using CommandLine;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace End2End.E2ETests;

public class TestCopying
{
    [Fact]
    public async Task TestCopyOutFromMss()
    {
        const string tableName = "AllTypes";
        var cmdArguments = GetArguments([
            "-d",
            "out",
            "-r",
            "Mss",
            "-c",
            "Server=.;Database=EndToEndTestABulkCopy;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
            "-l",
            ".\\local_out.log",
            "-f",
            ".\\",
            "-s",
            tableName
        ]);
        cmdArguments.Should().NotBeNull("because there are command line arguments");

        var configuration = new ConfigHelper().GetConfiguration(
            userSecretsKey: "128e015d-d8ef-4ca8-ba79-5390b26c675f",
            cmdArguments!.ToAppSettings());

        var services = new ServiceCollection();
        services.ConfigureServices(Rdbms.Mss, configuration);

        IServiceProvider provider = services.BuildServiceProvider();

        var copyOut = provider.GetRequiredService<ICopyOut>();
        await copyOut.RunAsync(CancellationToken.None);


        var schemaFile = await File.ReadAllTextAsync(tableName + ".schema");
        var dataFile = await File.ReadAllTextAsync(tableName + ".data");
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