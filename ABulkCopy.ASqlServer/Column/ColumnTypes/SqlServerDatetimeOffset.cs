namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerDatetimeOffset : DefaultColumn
{
    public SqlServerDatetimeOffset(int id, string name, bool isNullable, int? scale = 7)
        : base(id, name, isNullable)
    {
        Type = ColumnType.DateTimeOffset;
        Scale = scale??7;
        SetPrecisionAndLength(scale??7);
    }

    public override string GetNativeType()
    {
        return Scale == 7 ? "datetimeoffset" : $"datetimeoffset({Scale})";
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
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int scale)
    {
        Precision = 27 + scale;
        Length = Precision switch
        {
            < 30 => 8,
            < 32 => 9,
            _ => 10
        };
    }
}