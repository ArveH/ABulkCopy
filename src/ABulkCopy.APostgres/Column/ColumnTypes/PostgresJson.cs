namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresJson : PgTemplateStrColumn
{
    public PostgresJson(int id, string name, bool isNullable, string type = PgTypes.Json, int length = 0)
        : base(id, type, name, isNullable, length)
    {
    }

    public override string GetTypeClause()
    {
        return Type;
    }

    public override string ToString(object value)
    {
        // json/jsonb values must not be right-trimmed
        return InternalToString(value, shouldTrim: false);
    }
}
