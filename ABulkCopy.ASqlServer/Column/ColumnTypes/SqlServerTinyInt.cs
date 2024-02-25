namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerTinyInt : TemplateNumberColumn
{
    public SqlServerTinyInt(
        int id, string name, bool isNullable)
        : base(id, MssTypes.TinyInt, name, isNullable, 1, 3)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToByte(value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return Convert.ToByte(value);
    }
}