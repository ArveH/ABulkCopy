namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class MssDefaultColumn : DefaultColumn
{
    public MssDefaultColumn(int id, string type, string name, bool isNullable) : base(id, type, name, isNullable)
    {
    }

    protected override string GetIdentityClause() {
        return Identity != null ? $" IDENTITY({Identity.Seed},{Identity.Increment})" : string.Empty;
    }
}