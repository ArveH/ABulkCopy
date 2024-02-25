namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerDate : MssDefaultColumn
{
    public SqlServerDate(int id, string name, bool isNullable)
        : base(id, MssTypes.Date, name, isNullable)
    {
        Length = 3;
        Precision = 10;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}