namespace ABulkCopy.Common.Types.Index;

public class IndexHeader
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public required string Name { get; set; }
    public IndexType Type { get; set; }
    public required string Location { get; set; }
    public bool IsUnique { get; set; }

    public IndexHeader Clone()
    {
        return new IndexHeader
        {
            Id = Id,
            TableId = TableId,
            Name = Name,
            Type = Type,
            Location = Location,
            IsUnique = IsUnique
        };
    }
}