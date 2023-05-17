namespace ASqlServer.ColumnTypes;

public class TemplateSqlServerImageColumn: TemplateSqlServerVarBinaryColumn
{
    public TemplateSqlServerImageColumn(string name, bool isNullable, string def)
        : base(name, -1, isNullable, def)
    {
        Type = ColumnType.OldBlob;
    }

    public override string InternalTypeName()
    {
        return "image";
    }

    public override string ToString(object value)
    {
        throw new NotImplementedException("Column.ToFile for OLDBLOB");
    }

    public override object ToInternalType(string value)
    {
        throw new NotImplementedException("Column.ToInternalType for OLDBLOB");
    }
}