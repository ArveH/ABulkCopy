namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerImage : MssDefaultColumn
{
    public SqlServerImage(int id, string name, bool isNullable)
        : base(id, MssTypes.Image, name, isNullable)
    {
        Length = -1;
    }

    public override string ToString(object value)
    {
        throw new NotImplementedException("Raw column values can't be converted to strings");
    }

    public override object ToInternalType(string value)
    {
        throw new NotImplementedException("Raw column values are never represented as strings");
    }
}