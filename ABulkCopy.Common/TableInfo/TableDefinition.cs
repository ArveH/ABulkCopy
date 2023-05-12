namespace ABulkCopy.Common.TableInfo;

public class TableDefinition
{
    public required TableHeader  Header { get; set; }

    public List<ColumnDefinition> Columns { get; set; } = new();
    public PrimaryKey? PrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = new();
}