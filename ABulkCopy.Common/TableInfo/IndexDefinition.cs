namespace ABulkCopy.Common.TableInfo;

public class IndexDefinition
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string TableName { get; set; }
    public required string Location { get; set; }
    public bool IsUnique { get; set; }
    public bool IsClustered { get; set; }
    public List<OrderColumn> Columns { get; set; } = new();
}