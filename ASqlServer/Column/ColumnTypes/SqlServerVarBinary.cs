namespace ASqlServer.Column.ColumnTypes;

public class SqlServerVarBinary : TemplateSqlServerColumn
{
    public SqlServerVarBinary(int id, string name, bool isNullable, int length)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Raw;
        Length = length;
        if (Length > 8000)
        {
            Length = -1;
        }
    }

    public override string InternalTypeName()
    {
        return "varbinary";
    }

    public override string ToString(object value)
    {
        throw new NotImplementedException("Raw column values can't be converted to strings");
    }

    public override object ToInternalType(string value)
    {
        throw new NotImplementedException("Raw column values are never represented as strings");
    }

    public override Type GetDotNetType()
    {
        return typeof(byte[]);
    }
}