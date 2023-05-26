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

    public bool Connect()
    {
        return false;
    }
}