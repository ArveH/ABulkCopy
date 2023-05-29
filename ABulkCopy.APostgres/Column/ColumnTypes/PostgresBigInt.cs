namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresBigInt : TemplateNumberColumn
{
    public PostgresBigInt(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 8, 19)
    {
        Type = ColumnType.BigInt;
    }

    public override string GetNativeType()
    {
        return "bigint";
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