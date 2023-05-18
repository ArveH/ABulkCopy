namespace ASqlServer.Column.ColumnTypes;

public class SqlServerInt : TemplateNumberColumn
{
    public SqlServerInt(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 4, 10)
    {
        Type = ColumnType.Int;
    }

    public override string InternalTypeName()
    {
        return "int";
    }

    public override string ToString(object value)
    {
        return Convert.ToInt32(value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return Convert.ToInt32(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(int);
    }
}