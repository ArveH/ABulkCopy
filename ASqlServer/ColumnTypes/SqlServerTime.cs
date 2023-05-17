namespace ASqlServer.ColumnTypes;

public class SqlServerTime: TemplateSqlServerColumn
{
    public SqlServerTime(int id, string name, bool isNullable, int scale=7)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Time;
        Scale = scale;
        SetPrecisionAndLength(scale);
    }

    public override string InternalTypeName()
    {
        return "time";
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "HH:mm:ss.0000000", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int scale)
    {
        Precision = 9 + scale;
        Length = Precision switch
        {
            < 12 => 3,
            < 14 => 4,
            _ => 5
        };
    }
}