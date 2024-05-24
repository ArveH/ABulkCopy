using Microsoft.VisualStudio.TestPlatform.Utilities;
using Serilog.Events;

namespace CrossRDBMS.Tests.Helpers;

public class CopyContext
{
    private readonly List<string> _logMessages;
    private readonly IFileSystem _fileSystem;
    private readonly IServiceProvider _services;

    public CopyContext(
        Rdbms rdbms,
        CmdArguments? cmdArguments,
        List<string> logMessages,
        ITestOutputHelper output,
        IFileSystem fileSystem)
    {
        _logMessages = logMessages;
        _fileSystem = fileSystem;
        _services = GetServices(rdbms, output, cmdArguments);
    }

    public T GetServices<T>()
        where T : notnull
    {
        return _services.GetRequiredService<T>();
    }

    private IServiceProvider GetServices(
        Rdbms rdbms,
        ITestOutputHelper output,
        CmdArguments? cmdArguments)
    {
        if (cmdArguments == null)
        {
            throw new ApplicationException("Couldn't find command line arguments");
        }

        var configuration = new ConfigHelper().GetConfiguration(
            null, cmdArguments.ToAppSettings());

        IServiceCollection services = new ServiceCollection();
        services.ConfigureServices(rdbms, configuration);
        services.AddSingleton<ILogger>(new LoggerConfiguration()
            .WriteTo.TestOutput(output)
            .WriteTo.StringList(
                _logMessages,
                outputTemplate: "{Timestamp:yyyyMMdd HH:mm:ss.fff} [{Level:u3}] {Message:lj}  {Properties:j} {Exception}{NewLine}", 
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger());
        services.AddSingleton(_fileSystem);

        return services.BuildServiceProvider();
    }
}