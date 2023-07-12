namespace ABulkCopy.Common;

public class ImportState : IImportState
{
    private readonly ILogger _logger;
    private readonly ConcurrentBag<string> _tablesLeft;
    private readonly ConcurrentQueue<string> _tablesReadyForCreation;
    private readonly ConcurrentBag<string> _finishedTables;

    public ImportState(
        IEnumerable<string> tablesLeft,
        IEnumerable<string> tablesReadyForCreation,
        ILogger logger)
    {
        _tablesLeft = new(tablesLeft);
        _tablesReadyForCreation = new(tablesReadyForCreation);
        _finishedTables = new();
        _logger = logger.ForContext<ImportState>();
    }

    public bool IsTableFinished(string tableName)
    {
        return _finishedTables.Contains(tableName);
    }

    public bool TableReadyForCreation(string tableName)
    {
        if (_tablesLeft.TryTake(out var table))
        {
            _tablesReadyForCreation.Enqueue(table);
            return true;
        }

        _logger.Error("Table '{TableName}' was not found amongst the tables that where left", tableName);
        return false;
    }

    public void TableFinished(string tableName)
    {
        _finishedTables.Add(tableName);
    }

    public async IAsyncEnumerable<string> GetTablesReadyForCreation()
    {
        while (true)
        {
            if (_tablesReadyForCreation.TryDequeue(out var tableName))
            {
                yield return tableName;
            }
            else
            {
                if (!_tablesLeft.Any())
                {
                    yield break;
                }
                await Task.Delay(100);
            }
        }
    }
}