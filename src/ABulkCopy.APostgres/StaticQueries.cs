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
            "WHERE a.attrelid = @TableId\n" +
            "  AND a.attidentity IN ('a', 'd')\n" +
            "  AND NOT a.attisdropped\n";
    }

    public static string GetColumnInfo()
    {
        return
            "SELECT att.attnum AS column_id,\n" +
            "       att.attname AS column_name,\n" +
            "       typ.typname AS type_name,\n" +
            "       CASE WHEN typ.typname IN ('varchar', 'bpchar') THEN (att.atttypmod - 4) ELSE att.attlen END AS length,\n" +
            "       CASE WHEN typ.typname = 'numeric' THEN ((att.atttypmod - 4) >> 16) ELSE NULL END AS precision,\n" +
            "       CASE WHEN typ.typname = 'numeric' THEN (att.atttypmod - 4) & 65535 ELSE NULL END AS scale,\n" +
            "       NOT att.attnotnull AS is_nullable,\n" +
            "       CASE WHEN co.collname = 'default' THEN db.datcollate ELSE co.collname END AS collation_name,\n" +
            "       pg_get_expr(ad.adbin, ad.adrelid) AS column_default\n" +
            "FROM pg_attribute att\n" +
            "JOIN pg_class cls ON cls.oid = att.attrelid\n" +
            "JOIN pg_type typ ON typ.oid = att.atttypid\n" +
            "LEFT JOIN pg_attrdef ad ON ad.adrelid = att.attrelid AND ad.adnum = att.attnum\n" +
            "LEFT JOIN pg_collation co ON att.attcollation = co.oid\n" +
            "JOIN pg_database db ON db.datname = current_database()\n" +
            "WHERE att.attnum > 0 AND NOT att.attisdropped and cls.oid = @TableId\n" +
            "ORDER BY att.attnum;\n";
    }
}