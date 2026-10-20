namespace ABulkCopy.Common.Types.Table;

public class TableHeader
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Schema { get; set; }
    public string? Location { get; set; }
    public bool IsCaseSensitive { get; set; }
    public Identity? Identity { get; set; }
}