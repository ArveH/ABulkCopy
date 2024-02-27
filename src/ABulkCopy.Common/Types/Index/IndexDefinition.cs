namespace ABulkCopy.Common.Types.Index;

public class IndexDefinition
{
    public required IndexHeader Header { get; set; }
    public List<IndexColumn> Columns { get; set; } = new();

    public IndexDefinition Clone()
    {
        return new IndexDefinition
        {
            Header = Header.Clone(),
            Columns = Columns.Select(x => x.Clone()).ToList()
        };
    }
}