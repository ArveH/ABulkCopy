namespace ASqlServer.Column.ColumnTypes;

public class SqlServerUniqueIdentifier : DefaultColumn
{
    public SqlServerUniqueIdentifier(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Guid;
        Length = 16;
    }

    public override string GetNativeType()
    {
        return "uniqueidentifier";
    }

    public override string ToString(object value)
    {
        return ((Guid)value).ToString();
    }

    public override object ToInternalType(string value)
    {
        return new Guid(value);
    }
}