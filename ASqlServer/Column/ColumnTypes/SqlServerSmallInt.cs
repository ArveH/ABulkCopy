namespace ASqlServer.Column.ColumnTypes;

public class SqlServerSmallInt : TemplateNumberColumn
{
    public SqlServerSmallInt(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 2, 5)
    {
        Type = ColumnType.SmallInt;
    }

    public override string InternalTypeName()
    {
        return "smallint";
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