namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerDateTime : MssDefaultColumn
{
    public SqlServerDateTime(int id, string name, bool isNullable)
        : base(id, MssTypes.DateTime, name, isNullable)
    {
        Length = 8;
    }

    public override string ToString(object value)
    {
        return Convert.ToDateTime(value).ToString("O", CultureInfo.InvariantCulture);
    }

    public override object ToInternalType(string value)
    {
        return DateTime.ParseExact(value, "O", CultureInfo.InvariantCulture);
    }

    public override Type GetDotNetType()
    {
        return typeof(DateTime);
    }
}