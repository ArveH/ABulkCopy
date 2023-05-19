namespace ASqlServer.Column.ColumnTypes;

// NOTE: Decimal and Numeric are synonyms in SQL Server,
// and can be used interchangeably.
public class SqlServerDecimal : DefaultColumn
{
    public SqlServerDecimal(
        int id, string name, bool isNullable, int precision, int? scale=0)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Decimal;
        Precision = precision;
        Scale = scale;
        Length = CalculateLength(precision);
    }

    public override string GetNativeType()
    {
        return Scale == 0 ? $"decimal({Precision})" : $"decimal({Precision}, {Scale})";
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

    private int CalculateLength(int precision)
    {
        return precision switch
        {
            < 10 => 5,
            < 20 => 9,
            < 29 => 13,
            _ => 17
        };
    }
}

