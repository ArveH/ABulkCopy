namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PgDefaultColumn : DefaultColumn
{
    public PgDefaultColumn(int id, string type, string name, bool isNullable) : base(id, type, name, isNullable)
    {
    }

    public override string GetIdentityClause()
    {
        return Identity == null ? string.Empty
            : $" GENERATED ALWAYS AS IDENTITY (INCREMENT BY {Identity.Increment} START WITH {Identity.Seed})";
    }
}