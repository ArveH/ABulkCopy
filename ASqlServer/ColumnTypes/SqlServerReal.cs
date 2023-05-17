namespace ASqlServer.ColumnTypes;

public class SqlServerReal : TemplateSqlServerColumn
{
    public SqlServerReal(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.SmallFloat;
        Length = 4;
        Precision = 24;
    }

    public override string InternalTypeName()
    {
        return "real";
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