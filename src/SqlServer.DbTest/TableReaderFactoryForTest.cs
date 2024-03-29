namespace SqlServer.DbTest;

public class TableReaderFactoryForTest : ITableReaderFactory
{
    private readonly ISelectCreator _selectCreator;
    private readonly ILogger _logger;

    public TableReaderFactoryForTest(
        ISelectCreator selectCreator,
        ILogger logger)
    {
        _selectCreator = selectCreator;
        _logger = logger;
    }

    public ITableReader GetTableReader(IDbContext dbContext)
    {
            return new MssTableReader(_selectCreator, _logger)
            {
                ConnectionString = dbContext.ConnectionString
            };
    }
}