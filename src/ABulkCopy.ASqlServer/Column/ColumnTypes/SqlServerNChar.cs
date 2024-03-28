namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerNChar : TemplateStrColumn
{
    public SqlServerNChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, MssTypes.NChar, name, isNullable, length, collation)
    {
        if (Length != -1)
        {
            Length /= 2;
        }
    }

    public override string ToString(object value)
    {
        return InternalToString(value, false);
    }

    public override string GetTypeClause()
    {
        return $"{Type}({Length})";
    }
}