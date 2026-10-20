namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresJson : PgTemplateStrColumn
{
    public PostgresJson(int id, string name, bool isNullable, int length = 0)
        : base(id, PgTypes.Json, name, isNullable, length)
    {
    }

    public override string GetTypeClause()
    {
        return Type;
    }

    public override string ToString(object value)
    {
        // json stores the exact text, so it must not be right-trimmed
        return InternalToString(value, shouldTrim: false);
    }
}
