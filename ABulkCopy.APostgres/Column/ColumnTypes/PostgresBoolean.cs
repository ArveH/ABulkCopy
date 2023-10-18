namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresBoolean : PgTemplateNumberColumn
{
    public PostgresBoolean(
        int id, string name, bool isNullable)
        : base(id, PgTypes.Boolean, name, isNullable, 1)
    {
    }

    public override string ToString(object value)
    {
        return (bool)value ? "true" : "false";
    }

    public override object ToInternalType(string value)
    {
        return value is "1" or "true" or "True";
    }

    public override Type GetDotNetType()
    {
        return typeof(bool);
    }

    // TODO: Remove
    public string GetDefaultClause()
    {
        if (DefaultConstraint == null) return "";

        var trimmedDefault = DefaultConstraint.Definition.TrimParentheses();
        if (trimmedDefault == "0")
        {
            return " DEFAULT false";
        }

        if (trimmedDefault == "1")
        {
            return " DEFAULT true";
        }

        return $" DEFAULT {DefaultConstraint.Definition}";
    }
}