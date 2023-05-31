namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerBinary : DefaultColumn
{
    public SqlServerBinary(int id, string name, bool isNullable, int length)
        : base(id, MssTypes.Binary, name, isNullable)
    {
        Length = length;
    }

    public override string GetTypeClause()
    {
        return $"{MssTypes.Binary}({Length})";
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