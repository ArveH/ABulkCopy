namespace ASqlServer.ColumnTypes;

public class SqlServerSmallDatetime: TemplateSqlServerColumn
{
    public SqlServerSmallDatetime(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Length = 4;
    }

    public override string InternalTypeName()
    {
        return "smalldatetime";
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}