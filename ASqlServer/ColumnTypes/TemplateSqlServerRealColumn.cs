using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerRealColumn : TemplateSqlServerIntColumn
{
    public TemplateSqlServerRealColumn(string name, bool isNullable, string def)
        : base(name, isNullable, false, def)
    {
        Type = ColumnType.BinaryFloat;
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
        if (value == null)
        {
            return null;
        }
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(double);
    }
}