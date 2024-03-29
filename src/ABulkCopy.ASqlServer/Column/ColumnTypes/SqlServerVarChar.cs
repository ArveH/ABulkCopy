namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerVarChar : TemplateStrColumn
{
    public SqlServerVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, MssTypes.VarChar, name, isNullable, length, collation)
    {
    }

    public override string GetTypeClause()
    {
        return Length == -1 ? $"{Type}(max)" : $"{Type}({Length})";
    }
}