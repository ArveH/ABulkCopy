namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresUuid : PgDefaultColumn
{
    public PostgresUuid(int id, string name, bool isNullable)
        : base(id, PgTypes.Uuid, name, isNullable)
    {
    }

    public override string ToString(object value)
    {
        return ((Guid)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return new Guid(value);
    }

    protected override string GetDefaultClause()
    {
        if (DefaultConstraint == null) return "";

        if (DefaultConstraint.Definition.Contains("newid", StringComparison.InvariantCultureIgnoreCase))
        {
            return " DEFAULT gen_random_uuid()";
        }

        return $" DEFAULT {DefaultConstraint.Definition}";
    }
}