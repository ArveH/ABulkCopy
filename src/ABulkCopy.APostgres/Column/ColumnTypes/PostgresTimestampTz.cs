namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestampTz : PgDefaultColumn
{
    public PostgresTimestampTz(int id, string name, bool isNullable, int? precision=null)
        : base(id, PgTypes.TimestampTz, name, isNullable)
    {
        Precision = precision is null or > 6 or < 0 ? 6 : precision;
    }

    public override string GetTypeClause()
    {
        return Precision == 6 ? $"{Type}" : $"{Type}({Precision})";
    }

    public override string ToString(object value)
    {
        return ((DateTimeOffset)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTimeOffset);
    }
}