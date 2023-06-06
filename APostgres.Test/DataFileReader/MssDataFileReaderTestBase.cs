﻿namespace APostgres.Test.DataFileReader;

public class MssDataFileReaderTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly IConfiguration TestConfiguration;

    protected MssDataFileReaderTestBase(ITestOutputHelper output)
    {
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
    }
}