namespace ABulkCopy.Common.Utils;

public class SelectCreator : ISelectCreator
{
    private readonly ILogger _logger;

    public SelectCreator(ILogger logger)
    {
        _logger = logger;
    }

    public string CreateSelect(TableDefinition tableDefinition)
    {
        if (!tableDefinition.Columns.Any())
        {
            throw new ArgumentException(
                $"No columns in table definition for table " + 
                $"'{tableDefinition.Header.Name}'", nameof(tableDefinition.Columns));
        }
        
        var columns = tableDefinition.Columns
            .Select(c => c.Name).ToList();

        _logger.Information(
            "Created select statement for table '{TableName}' with '{ColumnCount}' columns",
            tableDefinition.Header.Name, columns.Count);
        return "SELECT " 
               + string.Join(", ", columns) 
               + $" FROM [{tableDefinition.Header.Schema}].[{tableDefinition.Header.Name}]";
    }
}