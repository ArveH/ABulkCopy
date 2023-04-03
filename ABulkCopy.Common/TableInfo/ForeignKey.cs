namespace ABulkCopy.Common.TableInfo;

public class ForeignKey
{
    public required string Name { get; set; }
    public required OrderColumn ColName { get; set; }
    public required string ColumnReference { get; set; }
    public required string TableReference { get; set; }
    public required string SchemaReference { get; set; }
    public required string DbReference { get; set; }
    public DeleteAction DeleteAction { get; set; } = DeleteAction.NoAction;
    public UpdateAction UpdateAction { get; set; } = UpdateAction.NoAction;
}