﻿namespace ASqlServer.Test;

public abstract class MssTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly IConfiguration TestConfiguration;
    protected readonly IDbContext MssDbContext;
    protected readonly IMssSystemTables MssSystemTables;

    protected MssTestBase(ITestOutputHelper output)
    {
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");

        MssDbContext = new MssContext(TestConfiguration);

        MssSystemTables = CreateMssSystemTables();
    }

    private IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = TestConfiguration.Check(TestConstants.Config.ConnectionString);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory();
        IMssSystemTables systemTables = new MssSystemTables(
            MssDbContext,
            colFactory, TestLogger);
        return systemTables;
    }
}