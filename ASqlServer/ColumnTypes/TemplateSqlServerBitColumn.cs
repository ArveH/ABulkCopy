namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerBitColumn : TemplateSqlServerIntColumn
{
    public TemplateSqlServerBitColumn(string name, bool isNullable)
        : base(name, isNullable)
    {
        Type = ColumnType.Bool;
    }

    public override string InternalTypeName()
    {
        return "bit";
    }

    public override string ToString(object value)
    {
        return (bool)value ? "1" : "0";
    }

    public override object ToInternalType(string value)
    {
        return value.Length == 1 ? value == "1" : Convert.ToBoolean(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(bool);
    }
}