namespace ASqlServer.Column.ColumnTypes;

public class SqlServerBinary : TemplateSqlServerColumn
{
    public SqlServerBinary(int id, string name, bool isNullable, int length)
        : base(id, name, isNullable)
    {
        Type = ColumnType.SmallRaw;
        Length = length;
    }

    public override string InternalTypeName()
    {
        return "binary";
    }

    public override string ToString(object value)
    {
        return Convert.ToBase64String((byte[])value);
    }

    public override object ToInternalType(string value)
    {
        return Convert.FromBase64String(value);
    }

    public override Type GetDotNetType()
    {
        return typeof(byte[]);
    }
}