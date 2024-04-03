namespace End2EndTests;

public class CopyProg
{
    public static IServiceProvider GetServices(
        Rdbms rdbms,
        string connectionString,
        string tableName)
    {
        var cmdArguments = GetArguments([
                "-d",
                "out",
                "-r",
                rdbms.ToString(),
                "-c",
                connectionString,
                "-l",
                ".\\local_out.log",
                "-f",
                ".\\",
                "-s",
                tableName
            ]);
        if (cmdArguments == null)
        {
            throw new ApplicationException("Couldn't find command line arguments");
        }

        var configuration = new ConfigHelper().GetConfiguration(
            null, cmdArguments.ToAppSettings());

        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices(Rdbms.Mss, configuration);
        return services.BuildServiceProvider();
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