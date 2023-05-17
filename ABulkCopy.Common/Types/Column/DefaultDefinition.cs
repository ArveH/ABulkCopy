namespace ABulkCopy.Common.Types.Column;

public class DefaultDefinition
{
    public required string Name { get; set; }
    public required string Definition { get; set; }
    public bool IsSystemNamed { get; set; } = true;
}