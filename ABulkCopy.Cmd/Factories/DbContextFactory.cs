namespace ABulkCopy.Cmd.Factories;

public static class DbContextFactory
{
    public static IDbContext GetContext(string connectionString)
    {
        return new MssContext()
        {
            ConnectionString = connectionString
        };
    }
}