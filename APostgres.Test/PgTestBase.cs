﻿using Microsoft.Extensions.Logging.Abstractions;

namespace APostgres.Test;

public class PgTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly IConfiguration TestConfiguration;
    protected readonly IPgContext PgContext;

    protected PgTestBase(ITestOutputHelper output)
    {
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");

        PgContext = new PgContext(new NullLoggerFactory())
        {
            ConnectionString = PgDbHelper.Instance.ConnectionString
        };
    }
}