namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresBigInt : PgTemplateNumberColumn
{
    public PostgresBigInt(
        int id, string name, bool isNullable)
        : base(id, MssTypes.BigInt, name, isNullable, 0, 64)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToInt64(value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return Convert.ToInt64(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(long);
    }
}