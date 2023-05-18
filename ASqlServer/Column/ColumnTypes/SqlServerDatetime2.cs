namespace ASqlServer.Column.ColumnTypes;

public class SqlServerDatetime2 : DefaultColumn
{
    public SqlServerDatetime2(int id, string name, bool isNullable, int scale = 7)
        : base(id, name, isNullable)
    {
        Type = ColumnType.DateTime;
        Scale = scale;
        SetPrecisionAndLength(scale);
    }

    public override string InternalTypeName()
    {
        return "datetime2";
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

    private void SetPrecisionAndLength(int scale)
    {
        Precision = 20 + scale;
        Length = Precision switch
        {
            < 23 => 6,
            < 25 => 7,
            _ => 8
        };
    }
}