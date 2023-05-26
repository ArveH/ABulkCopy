using Npgsql;

namespace ABulkCopy.APostgres;

public class PgCommand
{
    private readonly ILogger _logger;

    public PgCommand(
        IDbContext dbContext,
        ILogger logger)
    {
        _logger = logger.ForContext<PgCommand>();
    }

    public async Task<bool> Connect()
    {
        try
        {
            var connString = "Host=localhost;Username=postgres;Database=ABulkCopy";
            connString = "Server=127.0.0.1;Port=5432;Database=ABulkCopy;User Id=postgres;CommandTimeout=20;";
            connString = "Server=localhost;Port=5432;Database=ABulkCopy;User Id=postgres;Integrated Security=true;";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();

            await using (var cmd = new NpgsqlCommand("INSERT INTO testing (col1) VALUES (@p)", conn))
            {
                cmd.Parameters.AddWithValue("p", 123);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new NpgsqlCommand("SELECT col1 FROM testing", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var val = reader.GetInt32(0);
                    if (val == 123)
                    {
                        _logger.Information("Found inserted row with value 123");
                        return true;
                    }
                }
            }
            _logger.Information("Didn't find inserted row with value 123");
            return false;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return false;
        }
    }
}