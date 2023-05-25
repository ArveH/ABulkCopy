namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerTime : DefaultColumn
{
    public SqlServerTime(int id, string name, bool isNullable, int? scale = 7)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Time;
        Scale = scale??7;
        SetPrecisionAndLength(scale??7);
    }

    public override string GetNativeType()
    {
        return Scale == 7 ? "time" : $"time({Scale})";
    }

    public override string ToString(object value)
    {
        return ((TimeSpan)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return TimeSpan.ParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int? scale)
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