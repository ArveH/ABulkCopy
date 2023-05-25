using ABulkCopy.Common.Config;
using ABulkCopy.TestData.Initialization;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ABulkCopy.TestData;

public class DesignTimeContextFactoryForPostgres: IDesignTimeDbContextFactory<IdsTestingContext>
{
    public IdsTestingContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"Current dir: {Directory.GetCurrentDirectory()}");

        IConfiguration configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        var connectionString = configuration.GetConnectionString(Constants.Config.DbKey);
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        var builder = new DbContextOptionsBuilder<IdsTestingContext>();
        builder
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseNpgsql(
                connectionString,
                options =>
                {
                    options.MigrationsAssembly("ABulkCopy.TestData");
                });

        return new IdsTestingContext(builder.Options);
    }
}