namespace ASqlServer.Column.ColumnTypes;

public class SqlServerTinyInt : TemplateNumberColumn
{
    public SqlServerTinyInt(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 1, 3)
    {
        Type = ColumnType.TinyInt;
    }

    public override string InternalTypeName()
    {
        return "tinyint";
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