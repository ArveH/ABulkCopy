namespace ASqlServer.Column.ColumnTypes;

public class SqlServerMoney : TemplateNumberColumn
{
    public SqlServerMoney(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 8, 19, 4)
    {
        Type = ColumnType.Money;
    }

    public override string InternalTypeName()
    {
        return "money";
    }

    public override string ToString(object value)
    {
        // The # removes trailing zero. Will round up last number if more than 4 decimals. 
        return Convert.ToDecimal(value).ToString("0.####", CultureInfo.InvariantCulture);
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