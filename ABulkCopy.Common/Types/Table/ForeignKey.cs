namespace ABulkCopy.Common.Types.Table;

public class ForeignKey
{
    public required string Name { get; set; }
    public required string ColName { get; set; }
    public required string ColumnReference { get; set; }
    public required string TableReference { get; set; }
    public DeleteAction DeleteAction { get; set; } = DeleteAction.NoAction;
    public UpdateAction UpdateAction { get; set; } = UpdateAction.NoAction;
}