namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresInt : PgTemplateNumberColumn
{
    public PostgresInt(
        int id, string name, bool isNullable)
        : base(id, PgTypes.Int, name, isNullable, 0, 32)
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