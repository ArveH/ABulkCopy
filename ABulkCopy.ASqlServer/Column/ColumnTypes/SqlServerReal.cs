namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerReal : DefaultColumn
{
    public SqlServerReal(int id, string name, bool isNullable)
        : base(id, MssTypes.Real, name, isNullable)
    {
        Length = 4;
        Precision = 24;
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