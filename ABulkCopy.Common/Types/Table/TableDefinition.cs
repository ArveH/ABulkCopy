namespace ABulkCopy.Common.Types.Table;

public class TableDefinition
{
    public TableDefinition()
    {
        
    }

    public TableDefinition(Rdbms rdbms)
    {
        Rdbms = rdbms;
    }
    public Rdbms Rdbms { get; }
    public required TableHeader Header { get; set; }

    public List<IColumn> Columns { get; set; } = new();
    public PrimaryKey? PrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = new();
    public List<IndexDefinition> Indexes { get; set; } = new();
}