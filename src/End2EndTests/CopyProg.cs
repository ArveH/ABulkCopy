namespace End2EndTests;

public class CopyProg
{
    public static IServiceProvider GetServices(
        CopyDirection direction,
        Rdbms rdbms,
        string connectionString,
        string? searchString=null)
    {
        List<string> args = [
            "-d",
            direction.ToString(),
            "-r",
            rdbms.ToString(),
            "-c",
            connectionString,
            "-l",
            ".\\local_out.log",
            "-f",
            ".\\"
        ];
        if (searchString != null)
        {
            args.Add("-s");
            args.Add(searchString);
        }

        var cmdArguments = GetArguments(args.ToArray());
        if (cmdArguments == null)
        {
            throw new ApplicationException("Couldn't find command line arguments");
        }

        var configuration = new ConfigHelper().GetConfiguration(
            null, cmdArguments.ToAppSettings());

        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices(rdbms, configuration);
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