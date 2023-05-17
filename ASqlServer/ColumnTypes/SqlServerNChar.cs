namespace ASqlServer.ColumnTypes;

public class SqlServerNChar : TemplateStrColumn
{
    public SqlServerNChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, name, isNullable, length, collation)
    {
        Type = ColumnType.NChar;
    }

    public override string InternalTypeName()
    {
        return "nchar";
    }
}