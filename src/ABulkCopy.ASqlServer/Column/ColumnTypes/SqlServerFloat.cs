namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerFloat : MssDefaultColumn
{
    public SqlServerFloat(int id, string name, bool isNullable, int precision = 53)
        : base(id, MssTypes.Float, name, isNullable)
    {
        if (precision < 25)
        {
            Type = MssTypes.Real;
            Length = 4;
            Precision = 24;
        }
        else
        {
            Type = MssTypes.Float;
            Length = 8;
            Precision = 53;
        }
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