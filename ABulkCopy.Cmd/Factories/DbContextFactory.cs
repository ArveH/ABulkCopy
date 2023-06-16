using Microsoft.Extensions.Logging;

namespace ABulkCopy.Cmd.Factories;

public static class DbContextFactory
{
    public static IDbContext GetContext(string connectionString)
    {
        if (connectionString.Contains("Server="))
        {
            return new MssContext()
            {
                ConnectionString = connectionString
            };
        }
        else
        {
            return new PgContext(new LoggerFactory().AddSerilog(Log.Logger))
            {
                ConnectionString = connectionString
            };
        }
    }
}