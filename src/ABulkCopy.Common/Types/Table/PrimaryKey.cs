namespace ABulkCopy.Common.Types.Table;

public class PrimaryKey
{
    public string? Name { get; set; }
    public List<OrderColumn> ColumnNames { get; set; } = new();

    public PrimaryKey Clone() {
        return new PrimaryKey
        {
            Name = Name,
            ColumnNames = ColumnNames.Select(x => x.Clone()).ToList()
        };
    }
}