using ABulkCopy.Common.Config;
using CreateMssTestDatabase.Initialization;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CreateMssTestDatabase;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<IdsTestingContext>
{
    public IdsTestingContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"Current dir: {Directory.GetCurrentDirectory()}");

        IConfiguration configuration = new ConfigHelper().GetConfiguration();
        var connectionString = configuration.GetConnectionString(Constants.Config.DbKey);
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));
        var assemblyName = typeof(DesignTimeContextFactory).Assembly.GetName().Name;
        Console.WriteLine($"Assembly name: {assemblyName}");
        Console.WriteLine($"Mss Connection string: {connectionString}");
        var builder = new DbContextOptionsBuilder<IdsTestingContext>();
        builder
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseSqlServer(
                connectionString,
                options =>
                {
                    options.MigrationsAssembly(assemblyName);
                });

        return new IdsTestingContext(builder.Options);
    }
}