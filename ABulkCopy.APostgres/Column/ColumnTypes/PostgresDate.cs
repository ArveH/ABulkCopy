namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresDate : DefaultColumn
{
    public PostgresDate(int id, string name, bool isNullable)
        : base(id, PgTypes.Date, name, isNullable)
    {
        Length = 0;
        Scale = 0;
        Precision = 0;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}