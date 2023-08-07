using ABulkCopy.Common.Graph;

namespace ABulkCopy.Common;

public interface ITableSequencer
{
    bool IsTableFinished(string tableName);
    bool TableReadyForCreation(string tableName);
    void TableFinished(INode node);
    IAsyncEnumerable<INode> GetTablesReadyForCreation();
}