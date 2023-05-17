namespace ASqlServer.ColumnTypes;

public class SqlServerChar : TemplateStrColumn
{
    public SqlServerChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, name, isNullable, length, collation)
    {
        Type = ColumnType.Char;
    }

    public override string InternalTypeName()
    {
        return "char";
    }
}