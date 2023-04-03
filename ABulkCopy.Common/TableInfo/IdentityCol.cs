namespace ABulkCopy.Common.TableInfo;

public class IdentityCol
{
    public required string Name { get; set; }
    public int Seed { get; set; } = 1;
    public int Increment { get; set; } = 1;
}