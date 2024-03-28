namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerNVarChar : TemplateStrColumn
{
    public SqlServerNVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, MssTypes.NVarChar, name, isNullable, length, collation)
    {
    }

    public override string GetTypeClause()
    {
        return Length == -1 ? $"{Type}(max)" : $"{Type}({Length})";
    }
}