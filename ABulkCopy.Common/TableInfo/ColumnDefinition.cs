namespace ABulkCopy.Common.TableInfo;

public class ColumnDefinition
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string DataType { get; set; }
    public bool Computed { get; set; }
    public bool Nullable { get; set; }
    public int Length { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public string? Collation { get; set; }
}