namespace ABulkCopy.Common.Tree;

public class TreeDoctor
{
    public void Add(Root root, TableDefinition tableDefinition)
    {
        if (tableDefinition.ForeignKeys.Count == 0)
        {
            root.Children.Add(new Node(tableDefinition));
            return;
        }
    }
}