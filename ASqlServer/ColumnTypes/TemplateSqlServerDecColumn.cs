using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerDecColumn: TemplateSqlServerIntColumn
{
    private readonly string _typeToString;

    public TemplateSqlServerDecColumn(string name, int prec, int scale, bool isNullable, bool isIdentity, string def)
        : base(name, isNullable, isIdentity, def)
    {
        Details["Prec"] = prec;
        Details["Scale"] = scale;
        Type = ColumnType.Dec;
        _typeToString = $"dec({prec},{scale})";
    }

    public override string InternalTypeName()
    {
        return _typeToString;
    }

    public override string ToString(object value)
    {
        // The # removes trailing zero. Will round up last number if more than 8 decimals. 
        return Convert.ToDecimal(value).ToString("0.########", CultureInfo.InvariantCulture);
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
        return typeof(decimal);
    }

}