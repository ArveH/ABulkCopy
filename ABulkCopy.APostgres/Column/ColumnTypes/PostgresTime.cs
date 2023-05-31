namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTime : DefaultColumn
{
    public PostgresTime(int id, string name, bool isNullable, int? precision = 6)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Time;
        Precision = precision??6;
        Length = 8;
    }

    public override string GetNativeType()
    {
        return Scale == 6 ? "time" : $"time({Scale})";
    }

    public override string ToString(object value)
    {
        return ((TimeSpan)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return TimeSpan.ParseExact(value, "HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(TimeOnly);
    }
}