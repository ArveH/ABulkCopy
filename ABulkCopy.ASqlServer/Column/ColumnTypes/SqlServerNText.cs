namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerNText : TemplateStrColumn
{
    public SqlServerNText(int id, string name, bool isNullable, string? collation = null)
        : base(id, MssTypes.NText, name, isNullable, -1, collation)
    {
    }
}