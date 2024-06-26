namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestamp : PgDefaultColumn
{
    public PostgresTimestamp(int id, string name, bool isNullable, int? precision = null)
        : base(id, PgTypes.Timestamp, name, isNullable)
    {
        Precision = precision is null or > 6 or < 0 ? 6 : precision;
    }

    public override string GetTypeClause()
    {
        return Precision == 6 ? $"{Type}" : $"{Type}({Precision})";
    }

    public override string ToString(object value)
    {
        return ((DateTime)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        // We remove the 'Z' at the end of the string because a datetime is always stored in UTC,
        // and we don't want the time to be adjusted to the local time.
        // NOTE: Postgres recommends to always use timestamp with time zone
        // TODO: Consider making it an option to convert to local time or not
        return DateTime.Parse(value.TrimEnd('Z'), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}