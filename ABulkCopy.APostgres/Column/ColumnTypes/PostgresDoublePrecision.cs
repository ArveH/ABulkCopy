namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresDoublePrecision: DefaultColumn
{
    public PostgresDoublePrecision(int id, string name, bool isNullable)
        : base(id, PgTypes.DoublePrecision, name, isNullable)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(double);
    }
}