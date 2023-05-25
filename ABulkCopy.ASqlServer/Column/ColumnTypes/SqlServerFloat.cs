namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerFloat : DefaultColumn
{
    private readonly string _typeName;

    public SqlServerFloat(int id, string name, bool isNullable, int precision = 53)
        : base(id, name, isNullable)
    {
        if (precision < 25)
        {
            Type = ColumnType.SmallFloat;
            Length = 4;
            Precision = 24;
            _typeName = "real";
        }
        else
        {
            Type = ColumnType.Float;
            Length = 8;
            Precision = 53;
            _typeName = "float";
        }
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