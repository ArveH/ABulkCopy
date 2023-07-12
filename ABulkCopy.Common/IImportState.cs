namespace ABulkCopy.Common;

public interface IImportState
{
    bool IsTableFinished(string tableName);
    bool TableReadyForCreation(string tableName);
    void TableFinished(string tableName);
    IAsyncEnumerable<string> GetTablesReadyForCreation();
}