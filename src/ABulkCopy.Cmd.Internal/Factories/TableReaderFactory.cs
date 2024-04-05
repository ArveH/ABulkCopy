namespace ABulkCopy.Cmd.Internal.Factories;

public class TableReaderFactory : ITableReaderFactory
{
    private readonly ISelectCreator _selectCreator;
    private readonly ILogger _logger;

    public TableReaderFactory(
        ISelectCreator selectCreator,
        ILogger logger)
    {
        _selectCreator = selectCreator;
        _logger = logger;
    }

    public ITableReader GetTableReader(IDbContext dbContext)
    {
        if (dbContext.Rdbms == Rdbms.Mss)
            return new MssTableReader(_selectCreator, _logger) {ConnectionString = dbContext.ConnectionString};

        throw new NotSupportedException($"Rdbms {dbContext.Rdbms} is not supported");
    }
}