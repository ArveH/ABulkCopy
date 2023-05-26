namespace ABulkCopy.Common.Types.Index;

public class IndexDefinition
{
    public required IndexHeader Header { get; set; }
    public List<IndexColumn> Columns { get; set; } = new();
}