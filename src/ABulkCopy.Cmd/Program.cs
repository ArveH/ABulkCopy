namespace ABulkCopy.Cmd;

public class Program
{
    public static async Task Main(string[] args)
    {
        var cmdArguments = CmdArguments.Create(args);
        if (cmdArguments == null) return;
        var configuration = new ConfigHelper().GetConfiguration(
            userSecretsKey: "128e015d-d8ef-4ca8-ba79-5390b26c675f",
            cmdArguments.ToAppSettings());
        Log.Logger = LogConfigHelper.ConfigureLogger(configuration, configuration.Check(Constants.Config.LogFile));
        var version = Process.GetCurrentProcess().MainModule?.FileVersionInfo.ProductVersion;
        Log.Information("ABulkCopy.Cmd (version: {Version}) started.", version);
        Console.WriteLine($"ABulkCopy.Cmd (version: {version}) started.");

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddConfiguration(configuration);
            builder.ConfigureServices(cmdArguments.Rdbms, configuration);
            var host = builder.Build();

            if (!string.IsNullOrWhiteSpace(cmdArguments.PreScript))
            {
                if (!File.Exists(cmdArguments.PreScript))
                {
                    Log.Error("PreScript file '{PreScript}' does not exist", cmdArguments.PreScript);
                    Console.WriteLine($"PreScript file '{cmdArguments.PreScript}' does not exist");
                    return;
                }
                var scriptRunner = host.Services.GetRequiredService<IScriptRunner>();
                var (succeededCommands, failedCommands) = await scriptRunner.ExecuteAsync(cmdArguments.PreScript, cts.Token);
                Log.Information("Executed pre-script: {PreScript}. Succeeded: {Succeeded}, Failed: {Failed}",
                    cmdArguments.PreScript, succeededCommands, failedCommands);
            }

            if (cmdArguments.Direction == CopyDirection.In)
            {
                var copyIn = host.Services.GetRequiredService<ICopyIn>();
                await copyIn.RunAsync(cmdArguments.Rdbms, cts.Token);
            }
            else
            {
                var copyOut = host.Services.GetRequiredService<ICopyOut>();
                var folder = configuration.Check(Constants.Config.Folder);
                if (DataFolder.CreateIfNotExists(folder) == CmdStatus.ShouldExit)
                {
                    Console.WriteLine("ABulkCopy.Cmd finished.");
                    return;
                }

                await copyOut.RunAsync(cts.Token);
            }

            if (!string.IsNullOrWhiteSpace(cmdArguments.PostScript))
            {
                if (!File.Exists(cmdArguments.PostScript))
                {
                    Log.Error("PostScript file '{PreScript}' does not exist", cmdArguments.PostScript);
                    Console.WriteLine($"PostScript file '{cmdArguments.PostScript}' does not exist");
                    return;
                }
                var scriptRunner = host.Services.GetRequiredService<IScriptRunner>();
                var (succeededCommands, failedCommands) = await scriptRunner.ExecuteAsync(cmdArguments.PostScript, cts.Token);
                Log.Information("Executed post-script: {PostScript}. Succeeded: {Succeeded}, Failed: {Failed}",
                    cmdArguments.PostScript, succeededCommands, failedCommands);
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
}