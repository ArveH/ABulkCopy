namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTime : DefaultColumn
{
    public PostgresTime(int id, string name, bool isNullable, int? precision = 6)
        : base(id, PgTypes.Time, name, isNullable)
    {
        Length = 0;
        Scale = 0;
        Precision = precision is null or > 6 or < 0 ? 6 : precision;
    }

    public override string GetTypeClause()
    {
        return Scale == 6 ? $"{Type}" : $"{Type}({Scale})";
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