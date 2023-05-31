namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresDoublePrecision: DefaultColumn
{
    private readonly string _typeName;

    public PostgresDoublePrecision(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Float;
        Length = 8;
        Precision = 15;
        _typeName = "float";
    }

    public override string GetNativeType()
    {
        return _typeName;
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