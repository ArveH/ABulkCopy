namespace ABulkCopy.Common.Types.Table;

public class TableDefinition
{
    public required TableHeader Header { get; set; }

    public List<IColumn> Columns { get; set; } = new();
    public PrimaryKey? PrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = new();
}