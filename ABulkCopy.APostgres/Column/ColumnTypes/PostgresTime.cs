namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTime : DefaultColumn
{
    public PostgresTime(int id, string name, bool isNullable, int? precision = 6)
        : base(id, PgTypes.Time, name, isNullable)
    {
        Precision = precision is null or > 6 or < 0 ? 6 : precision;
    }

    public override string GetTypeClause()
    {
        return Scale == 6 ? $"{Type}" : $"{Type}({Precision})";
    }

    public override string ToString(object value)
    {
        return ((TimeSpan)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return TimeOnly.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(TimeOnly);
    }
}