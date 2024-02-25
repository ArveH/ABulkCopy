namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerSmallInt : TemplateNumberColumn
{
    public SqlServerSmallInt(
        int id, string name, bool isNullable)
        : base(id, MssTypes.SmallInt, name, isNullable, 2, 5)
    {
    }

    public override string ToString(object value)
    {
        return Convert.ToInt16(value).ToString();
    }

    public override Type GetDotNetType()
    {
        return typeof(short);
    }
}