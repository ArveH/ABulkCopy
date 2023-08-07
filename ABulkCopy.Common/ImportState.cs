using ABulkCopy.Common.Graph;

namespace ABulkCopy.Common;

public class ImportState : IImportState
{
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, INode> _tablesNotReadyForCreation;
    private readonly ConcurrentQueue<INode> _tablesReadyForCreation;
    private readonly ConcurrentBag<INode> _finishedTables;

    public ImportState(
        IEnumerable<INode> tablesLeft,
        IEnumerable<INode> tablesReadyForCreation,
        ILogger logger)
    {
        _tablesNotReadyForCreation = new (tablesLeft.ToDictionary(n => n.Name));
        _tablesReadyForCreation = new(tablesReadyForCreation);
        _finishedTables = new();
        _logger = logger.ForContext<ImportState>();
    }

    public bool IsTableFinished(string tableName)
    {
        return _finishedTables.Any(n => n.Name == tableName);
    }

    public bool TableReadyForCreation(string tableName)
    {
        if (_tablesNotReadyForCreation.TryRemove(tableName, out var node))
        {
            _logger.Information("IMPORTSTATE: Table '{TableName}' removed from NotReady", tableName);
            _tablesReadyForCreation.Enqueue(node);
            _logger.Information("IMPORTSTATE: Table '{TableName}' added to Ready", tableName);
            return true;
        }

        _logger.Error("IMPORTSTATE: Table '{TableName}' couldn't be removed from NotReady", tableName);

        return false;
    }

    public void TableFinished(INode node)
    {
        _finishedTables.Add(node);
        _logger.Information("IMPORTSTATE: Table '{TableName}' added to Finished", node.Name);
        foreach (var child in node.Children)
        {
            child.Value.Parents.Remove(node.Name);
            if (!child.Value.IsIndependent) continue;

            _tablesReadyForCreation.Enqueue(child.Value);
            _logger.Information("IMPORTSTATE: Table '{TableName}' added to Ready",
                child.Value.Name);
            if (_tablesNotReadyForCreation.TryRemove(child.Value.Name, out _))
            {
                _logger.Information("IMPORTSTATE: Table '{TableName}' removed from NotReady",
                    child.Value.Name);
            }
            else
            {
                _logger.Error("IMPORTSTATE: Table '{TableName}' couldn't be removed from NotReady",
                    child.Value.Name);
            }
        }
    }

    public async IAsyncEnumerable<INode> GetTablesReadyForCreation()
    {
        while (true)
        {
            if (_tablesReadyForCreation.TryDequeue(out var node))
            {
                _logger.Information("IMPORTSTATE: Table '{TableName}' removed from Ready",
                    node.Name);
                yield return node;
            }
            else
            {
                _logger.Information("IMPORTSTATE: No more tables Ready (this message can occur several times, while waiting for tables to finish)");
            }

            if (!_tablesNotReadyForCreation.Any())
            {
                _logger.Information("IMPORTSTATE: No more tables NotReady (this message should only occur once)");
                yield break;
            }

            await Task.Delay(100);
        }
    }
}