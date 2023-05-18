namespace ASqlServer.Column.ColumnTypes;

public class SqlServerDatetime : TemplateSqlServerColumn
{
    public SqlServerDatetime(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.DateTimeAlt;
        Length = 8;
    }

    public override string InternalTypeName()
    {
        return "datetime";
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}