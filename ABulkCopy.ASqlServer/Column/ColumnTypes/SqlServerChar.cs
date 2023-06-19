namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerChar : TemplateStrColumn
{
    public SqlServerChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, MssTypes.Char, name, isNullable, length, collation)
    {
    }

    public override string ToString(object value)
    {
        return InternalToString(value, false);
    }

    public override string GetTypeClause()
    {
        return $"{MssTypes.Char}({Length})";
    }
}