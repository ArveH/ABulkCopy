using ABulkCopy.Common.Identifier;

namespace ABulkCopy.Common.Utils;

public class SelectCreator : ISelectCreator
{
    private readonly IIdentifier _identifier;
    private readonly ILogger _logger;

    public SelectCreator(IIdentifier identifier, ILogger logger)
    {
        _identifier = identifier;
        _logger = logger;
    }

    public string CreateSelect(TableDefinition tableDefinition)
    {
        if (!tableDefinition.Columns.Any())
        {
            throw new ArgumentException(
                $"No columns in table definition for table " + 
                $"'{tableDefinition.GetFullName()}'", nameof(tableDefinition.Columns));
        }
        
        var columns = tableDefinition.Columns
            .Select(c => _identifier.Get(c.Name)).ToList();

        _logger.Debug(
            "Created select statement for table '{TableName}' with '{ColumnCount}' columns",
            tableDefinition.GetFullName(), columns.Count);
        return "SELECT " 
               + string.Join(", ", columns) 
               + $" FROM {_identifier.Get(tableDefinition.Header.Schema)}.{_identifier.Get(tableDefinition.Header.Name)}";
    }
}