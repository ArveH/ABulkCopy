namespace ABulkCopy.Common.Types.Column;

public class OrderColumn
{
    public required string Name { get; set; }
    public Direction Direction { get; set; } = Direction.Ascending;

    public OrderColumn Clone()
    {
        return new OrderColumn
        {
            Name = Name,
            Direction = Direction
        };
    }
}