namespace ABulkCopy.Common.Types.Table;

public class ForeignKey
{
    public string? ConstraintName { get; set; }
    public int? ConstraintId { get; set; }
    public required string TableReference { get; set; }
    public List<string> ColumnNames { get; set; } = new();
    public List<string> ColumnReferences { get; set; } = new();
    public DeleteAction DeleteAction { get; set; } = DeleteAction.NoAction;
    public UpdateAction UpdateAction { get; set; } = UpdateAction.NoAction;

    public ForeignKey Clone()
    {
        return new ForeignKey
        {
            ConstraintName = ConstraintName,
            TableReference = TableReference,
            ColumnNames = new List<string>(ColumnNames),
            ColumnReferences = new List<string>(ColumnReferences),
            DeleteAction = DeleteAction,
            UpdateAction = UpdateAction
        };
    }
}