namespace ASqlServer.Column.ColumnTypes;

public class SqlServerNVarChar : TemplateStrColumn
{
    public SqlServerNVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, name, isNullable, length, collation)
    {
        if (length > 4000)
        {
            Type = ColumnType.NLongText;
            Length = -1;
        }
        else
        {
            Type = ColumnType.NVarChar;
            Length = length;
        }
    }

    public override string GetNativeType()
    {
        return Length == -1 ? "nvarchar(max)" : $"nvarchar({Length})";
    }
}