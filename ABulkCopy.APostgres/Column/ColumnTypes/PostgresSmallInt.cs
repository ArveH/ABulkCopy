namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresSmallInt : TemplateNumberColumn
{
    public PostgresSmallInt(
        int id, string name, bool isNullable)
        : base(id, PgTypes.SmallInt, name, isNullable, 0, 16)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToInt32(value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return Convert.ToInt32(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(int);
    }
}