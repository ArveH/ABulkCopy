namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresBoolean : TemplateNumberColumn
{
    public PostgresBoolean(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 1)
    {
        Type = ColumnType.Bool;
    }

    public override string GetNativeType()
    {
        return "boolean";
    }

    public override string ToString(object value)
    {
        return (bool)value ? "true" : "false";
    }

    public override object ToInternalType(string value)
    {
        return value;
    }

    public override Type GetDotNetType()
    {
        return typeof(bool);
    }
}