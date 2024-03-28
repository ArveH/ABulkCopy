namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerDateTimeOffset : MssDefaultColumn
{
    public SqlServerDateTimeOffset(int id, string name, bool isNullable, int? scale = 7)
        : base(id, MssTypes.DateTimeOffset, name, isNullable)
    {
        Scale = scale??7;
        SetPrecisionAndLength(scale??7);
    }

    public override string GetTypeClause()
    {
        return Scale == 7 ? Type : $"{Type}({Scale})";
    }

    public override string ToString(object value)
    {
        return ((DateTimeOffset)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        return DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTimeOffset);
    }

    private void SetPrecisionAndLength(int scale)
    {
        Precision = scale switch
        {
            0 => 26,
            _ => 27 + scale
        };
        Length = Precision switch
        {
            < 30 => 8,
            < 32 => 9,
            _ => 10
        };
    }
}