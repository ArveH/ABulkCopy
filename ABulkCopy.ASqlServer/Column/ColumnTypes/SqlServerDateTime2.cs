namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerDateTime2 : DefaultColumn
{
    public SqlServerDateTime2(int id, string name, bool isNullable, int? scale = 7)
        : base(id, MssTypes.DateTime2, name, isNullable)
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
        var tmp = Convert.ToDateTime(value);
        return ((DateTime)value).ToString("O");
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "O", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int scale)
    {
        Precision = 20 + scale;
        Length = Precision switch
        {
            < 23 => 6,
            < 25 => 7,
            _ => 8
        };
    }
}