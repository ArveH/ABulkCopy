using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerFloatColumn : TemplateSqlServerIntColumn
{
    private readonly string _typeToString;

    public TemplateSqlServerFloatColumn(string name, int prec, bool isNullable, string def)
        : base(name, isNullable, false, def)
    {
        if (prec > 0)
        {
            Type = ColumnType.Float;
            Details["Prec"] = prec;
            _typeToString = $"float({prec})";
        }
        else
        {
            _typeToString = "float";
        }
    }

    public override string InternalTypeName()
    {
        return _typeToString;
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