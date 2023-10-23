namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresUuid : PgDefaultColumn
{
    public PostgresUuid(int id, string name, bool isNullable)
        : base(id, PgTypes.Uuid, name, isNullable)
    {
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