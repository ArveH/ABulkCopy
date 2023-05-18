namespace ASqlServer.Column.ColumnTypes;

public class SqlServerGuid : TemplateSqlServerColumn
{
    public SqlServerGuid(int id, string name, bool isNullable)
        : base(id, name, isNullable)
    {
        Type = ColumnType.Guid;
        Length = 16;
    }

    public override string InternalTypeName()
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

    public override Type GetDotNetType()
    {
        return typeof(Guid);
    }
}