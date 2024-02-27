namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresDoublePrecision: PgDefaultColumn
{
    public PostgresDoublePrecision(int id, string name, bool isNullable)
        : base(id, PgTypes.DoublePrecision, name, isNullable)
    {
        Precision = 53;
    }

    public override string ToString(object value)
    {
        return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return double.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(double);
    }
}