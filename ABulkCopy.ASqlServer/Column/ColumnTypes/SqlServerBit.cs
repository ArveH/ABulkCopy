namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerBit : TemplateNumberColumn
{
    public SqlServerBit(
        int id, string name, bool isNullable)
        : base(id, name, isNullable, 1, 1)
    {
        Type = ColumnType.Bool;
    }

    public override string GetNativeType()
    {
        return "bit";
    }

    public override string ToString(object value)
    {
        return (bool)value ? "1" : "0";
    }

    public override object ToInternalType(string value)
    {
        // TODO: Check if this is correct
        return value.Length == 1 ? value == "1" : Convert.ToBoolean(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(bool);
    }
}