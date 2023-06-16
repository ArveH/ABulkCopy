using ABulkCopy.Common.Config;
using ABulkCopy.Common.Types;
using ABulkCopy.TestData.Initialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ABulkCopy.TestData;

public class DesignTimeContextFactory: IDesignTimeDbContextFactory<IdsTestingContext>
{
    public IdsTestingContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"Current dir: {Directory.GetCurrentDirectory()}");

        IConfiguration configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        var connectionString = configuration.GetConnectionString(Constants.Config.DbKey);
        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        if (GetDbServer(connectionString!) == Rdbms.SqlServer)
        {
            var builder = new DbContextOptionsBuilder<IdsTestingContext>();
            builder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseSqlServer(
                    connectionString,
                    options =>
                    {
                        options.MigrationsAssembly("ABulkCopy.TestData");
                    });

            return new IdsTestingContext(builder.Options);
        }
        else
        {
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

    private Rdbms GetDbServer(string connectionString)
    {
        var canBePostgres = false;
        var canBeSqlServer = false;
        try
        {
            var _ = new NpgsqlConnectionStringBuilder(connectionString);
            canBePostgres = true;
        }
        catch
        {
            // ignored
        }
        try
        {
            var _ = new SqlConnectionStringBuilder(connectionString);
            canBeSqlServer = true;
        }
        catch
        {
            // ignored
        }

        if (canBeSqlServer && canBePostgres)
        {
            throw new ArgumentException("Connection string can be both Postgres and SqlServer");
        }

        if (!canBeSqlServer && !canBePostgres)
        {
            throw new ArgumentException("Connection string can't be parsed");
        }

        return canBePostgres ? Rdbms.Postgres : Rdbms.SqlServer;
    }
}