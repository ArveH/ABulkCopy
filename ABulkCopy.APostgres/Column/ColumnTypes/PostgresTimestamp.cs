namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestamp : DefaultColumn
{
    public PostgresTimestamp(int id, string name, bool isNullable, int? precision)
        : base(id, PgTypes.Timestamp, name, isNullable)
    {
        Precision = precision ?? 6;
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
        return DateTime.ParseExact(value, "O", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}