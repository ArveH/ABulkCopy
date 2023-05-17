namespace ASqlServer.ColumnTypes;

public class SqlServerDate: TemplateSqlServerColumn
{
    public SqlServerDate(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Date;
        Length = 3;
        Precision = 10;
    }

    public override string InternalTypeName()
    {
        return "date";
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}