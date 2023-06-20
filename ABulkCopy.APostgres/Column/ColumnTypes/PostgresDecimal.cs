namespace ABulkCopy.APostgres.Column.ColumnTypes;

// NOTE: Decimal and Numeric are equivalent in Postgres,
// and can be used interchangeably.
public class PostgresDecimal : PgDefaultColumn
{
    public PostgresDecimal(
        int id, string name, bool isNullable, int precision, int? scale=0)
        : base(id, PgTypes.Decimal, name, isNullable)
    {
        Precision = precision;
        Scale = scale;
        Length = CalculateLength(precision);
    }

    public override string GetTypeClause()
    {
        return Scale == 0 ? $"{Type}({Precision})" : $"{Type}({Precision}, {Scale})";
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

