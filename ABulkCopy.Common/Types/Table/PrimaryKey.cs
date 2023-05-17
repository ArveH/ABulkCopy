namespace ABulkCopy.Common.Types.Table;

public class PrimaryKey
{
    public required string Name { get; set; }
    public List<OrderColumn> ColumnNames { get; set; } = new();
}