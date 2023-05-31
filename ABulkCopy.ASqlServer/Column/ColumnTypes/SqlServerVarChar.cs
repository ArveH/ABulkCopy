namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerVarChar : TemplateStrColumn
{
    public SqlServerVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, MssTypes.VarChar, name, isNullable, length, collation)
    {
        if (length > 4000)
        {
            Length = -1;
        }
    }

    public override string GetTypeClause()
    {
        return Length == -1 ? $"{Type}(max)" : $"{Type}({Length})";
    }
}