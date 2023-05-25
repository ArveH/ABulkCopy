namespace ABulkCopy.Cmd.Factories;

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
        if (dbContext.DbServer == DbServer.SqlServer)
            return new MssTableReader(_selectCreator, _logger) {ConnectionString = dbContext.ConnectionString};

        throw new NotSupportedException($"DbServer {dbContext.DbServer} is not supported");
    }
}