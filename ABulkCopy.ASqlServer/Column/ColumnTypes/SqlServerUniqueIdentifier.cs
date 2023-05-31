namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerUniqueIdentifier : DefaultColumn
{
    public SqlServerUniqueIdentifier(int id, string name, bool isNullable)
        : base(id, MssTypes.UniqueIdentifier, name, isNullable)
    {
        Length = 16;
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