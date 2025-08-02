namespace ABulkCopy.APostgres;

public static class StaticQueries
{
    public static string GetTableIdAndLocation()
    {
        return
            "SELECT\n" +
            "    c.oid AS table_id,\n" +
            "    COALESCE(t.spcname, 'pg_default') AS tablespace\n" +
            "FROM pg_class c\n" +
            "JOIN pg_namespace n ON n.oid = c.relnamespace\n" +
            "LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace\n" +
            "WHERE n.nspname = @SchemaName\n" +
            "  AND c.relname = @TableName\n" +
            "  AND c.relkind = 'r';  -- 'r' = ordinary table\n";
    }

    public static string GetIdentityColumns()
    {
        return
            "SELECT a.attname, s.seqstart, s.seqincrement, a.attidentity\n" +
            "FROM pg_attribute a\n" +
            "JOIN pg_class c ON c.oid = a.attrelid\n" +
            "JOIN pg_depend d ON d.refobjid = c.oid AND d.refobjsubid = a.attnum\n" +
            "JOIN pg_class seq ON seq.oid = d.objid AND seq.relkind = 'S'\n" +
            "JOIN pg_sequence s ON s.seqrelid = seq.oid\n" +
            "WHERE a.attrelid = @TableId\n";
    }
}