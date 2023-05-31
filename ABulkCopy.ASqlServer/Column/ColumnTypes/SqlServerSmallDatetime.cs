namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerSmallDateTime : DefaultColumn
{
    public SqlServerSmallDateTime(int id, string name, bool isNullable)
        : base(id, MssTypes.SmallDateTime, name, isNullable)
    {
        Length = 4;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}