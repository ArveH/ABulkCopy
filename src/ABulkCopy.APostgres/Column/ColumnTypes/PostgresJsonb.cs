namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresJsonb : PgTemplateStrColumn
{
    public PostgresJsonb(int id, string name, bool isNullable, int length = 0)
        : base(id, PgTypes.Jsonb, name, isNullable, length)
    {
    }

    public override string GetTypeClause()
    {
        return Type;
    }

    public override string ToString(object value)
    {
        // jsonb is stored in a normalized binary form (no duplicate keys, keys
        // reordered, insignificant whitespace removed). Npgsql returns that
        // normalized text, so serialize it verbatim without right-trimming.
        return InternalToString(value, shouldTrim: false);
    }
}
