using System.Globalization;

namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerMoneyColumn : TemplateSqlServerIntColumn
{
    public TemplateSqlServerMoneyColumn(string name, bool isNullable)
        : base(name, isNullable)
    {
        Type = ColumnType.Money;
    }

    public override string InternalTypeName()
    {
        return "money";
    }

    public override string ToString(object value)
    {
        // The # removes trailing zero. Will round up last number if more than 15 decimals. 
        return Convert.ToDecimal(value).ToString("0.###############", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(decimal);
    }
}