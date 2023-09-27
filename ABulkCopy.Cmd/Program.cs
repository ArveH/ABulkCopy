namespace ABulkCopy.Cmd;

internal class Program
{
    static async Task Main(string[] args)
    {
        var cmdArguments = GetArguments(args);
        if (cmdArguments == null) return;
        var configuration = new ConfigHelper().GetConfiguration(
            userSecretsKey: "128e015d-d8ef-4ca8-ba79-5390b26c675f",
            cmdArguments.ToAppSettings());
        Log.Logger = LogConfigHelper.ConfigureLogger(configuration, configuration.Check(Constants.Config.LogFile));
        var version = Process.GetCurrentProcess().MainModule?.FileVersionInfo.ProductVersion;
        Log.Information("ABulkCopy.Cmd (version: {Version}) started.", version);
        Console.WriteLine($"ABulkCopy.Cmd (version: {version}) started.");

        try
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddConfiguration(configuration);
            builder.ConfigureServices(cmdArguments.Rdbms, configuration);
            var host = builder.Build();

            if (cmdArguments.Direction == CopyDirection.In)
            {
                var copyIn = host.Services.GetRequiredService<ICopyIn>();
                await copyIn.Run(cmdArguments.Rdbms);
            }
            else
            {
                var copyOut = host.Services.GetRequiredService<ICopyOut>();
                var folder = configuration.Check(Constants.Config.Folder);
                if (DataFolder.CreateIfNotExists(folder) == CmdStatus.ShouldExit)
                {
                    Log.Information("Folder {Folder} didn't exist. ABulkCopy.Cmd finished.",
                        folder);
                    Console.WriteLine($"Folder {folder} didn't exist. ABulkCopy.Cmd finished.");
                    return;
                }

                await copyOut.Run();
            }

            Log.Information("ABulkCopy.Cmd finished.");
            Console.WriteLine("ABulkCopy.Cmd finished.");
        }
        catch (Exception ex)
        {
            Console.Write("Host terminated unexpectedly: ");
            Console.WriteLine(ex.FlattenMessages());
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
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