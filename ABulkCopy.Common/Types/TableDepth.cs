namespace ABulkCopy.Common.Types;

public class TableDepth
{
    public TableDepth(string name, int depth)
    {
        Name = name;
        Depth = depth;
    }
    public string Name { get; set; }
    public int Depth { get; set; }
}