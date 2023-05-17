namespace ABulkCopy.Common.Types.Column;

public class ColumnDefinition
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string DataType { get; set; }
    public bool IsComputed => ComputedDefinition != null;
    public string? ComputedDefinition { get; set; }
    public bool IsNullable { get; set; }
    public Identity? Identity { get; set; }
    public int Length { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public bool HasDefault => DefaultConstraint != null;
    public DefaultDefinition? DefaultConstraint { get; set; }
    public string? Collation { get; set; }
}