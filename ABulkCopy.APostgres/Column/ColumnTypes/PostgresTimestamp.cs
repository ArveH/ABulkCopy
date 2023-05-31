namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresTimestamp : DefaultColumn
{
    public PostgresTimestamp(int id, string name, bool isNullable, int? precision)
        : base(id, name, isNullable)
    {
        Type = ColumnType.DateTime;
        Precision = precision ?? 6;
        Length = 8;
    }

    public override string GetNativeType()
    {
        return Precision == 6 ? "timestamp" : $"timestamp({Precision})";
    }

    public override string ToString(object value)
    {
        var tmp = Convert.ToDateTime(value);
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