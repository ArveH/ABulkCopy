using ABulkCopy.Common.Graph;

namespace ABulkCopy.Common;

public class ImportState : IImportState
{
    private readonly ILogger _logger;
    private readonly ConcurrentBag<string> _tablesNotReadyForCreation;
    private readonly ConcurrentQueue<string> _tablesReadyForCreation;
    private readonly ConcurrentBag<string> _finishedTables;

    public ImportState(
        IEnumerable<string> tablesLeft,
        IEnumerable<string> tablesReadyForCreation,
        ILogger logger)
    {
        _tablesNotReadyForCreation = new(tablesLeft);
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
        if (_tablesNotReadyForCreation.TryTake(out var table))
        {
            _logger.Information("IMPORTSTATE: Table '{TableName}' removed from NotReady", tableName);
            _tablesReadyForCreation.Enqueue(table);
            _logger.Information("IMPORTSTATE: Table '{TableName}' added to Ready", tableName);
            return true;
        }

        _logger.Error("IMPORTSTATE: Table '{TableName}' couldn't be removed from NotReady", tableName);

        return false;
    }

    public void TableFinished(Node node)
    {
        _finishedTables.Add(node.Name);
        _logger.Information("IMPORTSTATE: Table '{TableName}' added to Finished", node.Name);
        foreach (var child in node.Children)
        {
            child.Value.Parents.Remove(node.Name);
            if (!child.Value.IsIndependent) continue;

            _tablesReadyForCreation.Enqueue(child.Value.Name);
            _logger.Information("IMPORTSTATE: Table '{TableName}' added to Ready",
                child.Value.Name);
            if (_tablesNotReadyForCreation.TryTake(out var tableName))
            {
                _logger.Information("IMPORTSTATE: Table '{TableName}' removed from NotReady",
                    tableName);
            }
            else
            {
                _logger.Error("IMPORTSTATE: Table '{TableName}' couldn't be removed from NotReady",
                    child.Value.Name);
            }
        }
    }

    public async IAsyncEnumerable<string> GetTablesReadyForCreation()
    {
        while (true)
        {
            if (_tablesReadyForCreation.TryDequeue(out var tableName))
            {
                _logger.Information("IMPORTSTATE: Table '{TableName}' removed from Ready",
                    tableName);
                yield return tableName;
            }
            else
            {
                _logger.Information("IMPORTSTATE: No more tables Ready");
            }

            if (!_tablesNotReadyForCreation.Any())
            {
                _logger.Information("IMPORTSTATE: No more tables NotReady");
                yield break;
            }

            await Task.Delay(100);
        }
    }
}