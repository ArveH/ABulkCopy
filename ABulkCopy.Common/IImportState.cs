using ABulkCopy.Common.Graph;

namespace ABulkCopy.Common;

public interface IImportState
{
    bool IsTableFinished(string tableName);
    bool TableReadyForCreation(string tableName);
    void TableFinished(Node node);
    IAsyncEnumerable<string> GetTablesReadyForCreation();
}