namespace ABulkCopy.ASqlServer.Column.ColumnTypes;

public class SqlServerText : TemplateStrColumn
{
    public SqlServerText(int id, string name, bool isNullable, string? collation = null)
        : base(id, MssTypes.Text, name, isNullable, -1, collation)
    {
    }
}