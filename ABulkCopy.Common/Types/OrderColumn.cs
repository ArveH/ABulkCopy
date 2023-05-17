namespace ABulkCopy.Common.Types;

public class OrderColumn
{
    public required string Name { get; set; }
    public Direction Direction { get; set; } = Direction.Ascending;
}