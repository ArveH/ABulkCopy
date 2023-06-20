namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresText : PgTemplateStrColumn
{
    public PostgresText(int id, string name, bool isNullable, string? collation = null)
        : base(id, PgTypes.Text, name, isNullable, 1_073_741_824, collation)
    {
    }
}