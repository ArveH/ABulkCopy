using System.Diagnostics;

namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerTime : MssDefaultColumn
{
    public SqlServerTime(int id, string name, bool isNullable, int? scale = 7)
        : base(id, MssTypes.Time, name, isNullable)
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
        return ((TimeSpan)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return TimeSpan.ParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }

    private void SetPrecisionAndLength(int? scale)
    {
        Precision = scale switch
        {
            0 => 8,
            _ => 9 + scale
        };
        Length = Precision switch
        {
            < 12 => 3,
            < 14 => 4,
            _ => 5
        };
    }
}