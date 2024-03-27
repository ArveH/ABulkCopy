namespace SqlServer.DbTest;

public abstract class MssTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly IConfiguration TestConfiguration;
    protected readonly IDbContext MssDbContext;

    protected MssTestBase(ITestOutputHelper output)
    {
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");

        MssDbContext = new MssContext(TestConfiguration);
    }

    protected string GetName()
    {
        var st = new StackTrace();
        // Frames:
        //   0: GetName
        //   1: MoveNext
        //   2: Start
        //   3: <Should be the name of the test method>
        var sf = st.GetFrame(3);
        if (sf == null)
        {
            throw new InvalidOperationException("Stack Frame is null");
        }

        var methodName = sf.GetMethod()?.Name ?? throw new InvalidOperationException("Method is null");
        var methodName20 = methodName.Length > 24 ? methodName[4..24] : methodName;
        var machineName20 = Environment.MachineName.Length > 20 ? Environment.MachineName[..20] : Environment.MachineName;

        return machineName20 + methodName20;
    }
}