namespace ABulkCopy.Common.Types.Column;

public class DefaultDefinition
{
    public string? Name { get; set; }
    public required string Definition { get; set; }
    public bool IsSystemNamed { get; set; } = true;

    public DefaultDefinition Clone()
    {
        return new DefaultDefinition
        {
            Name = Name,
            Definition = Definition,
            IsSystemNamed = IsSystemNamed
        };
    }
}