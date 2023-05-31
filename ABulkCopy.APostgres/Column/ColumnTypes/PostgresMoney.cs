namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresMoney : DefaultColumn
{
    public PostgresMoney(
        int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Money;
    }

    public override string GetNativeType()
    {
        return "money";
    }

    public override string ToString(object value)
    {
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