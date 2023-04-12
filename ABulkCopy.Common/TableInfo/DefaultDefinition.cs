namespace ABulkCopy.Common.TableInfo;

public class DefaultDefinition
{
    public required string Name { get; set; }
    public required string Definition { get; set; }
    public bool IsSystemNamed { get; set; } = true;
}