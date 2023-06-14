namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresByteA : DefaultColumn
{
    public PostgresByteA(int id, string name, bool isNullable)
        : base(id, PgTypes.ByteA, name, isNullable)
    {
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