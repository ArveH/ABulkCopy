namespace ABulkCopy.APostgres;

public static class StaticQueries
{
    public static string GetTableIdAndLocation()
    {
        return
            "SELECT" + Environment.NewLine + 
            "    c.oid AS table_id," + Environment.NewLine + 
            "    COALESCE(t.spcname, 'pg_default') AS tablespace" + Environment.NewLine + 
            "FROM pg_class c" + Environment.NewLine + 
            "JOIN pg_namespace n ON n.oid = c.relnamespace" + Environment.NewLine + 
            "LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace" + Environment.NewLine + 
            "WHERE n.nspname = @SchemaName" + Environment.NewLine + 
            "  AND c.relname = @TableName" + Environment.NewLine + 
            "  AND c.relkind = 'r';  -- 'r' = ordinary table\n";
    }

    public static string GetIdentityColumns()
    {
        return
            "SELECT a.attname, s.seqstart, s.seqincrement, a.attidentity" + Environment.NewLine + 
            "FROM pg_attribute a" + Environment.NewLine + 
            "JOIN pg_class c ON c.oid = a.attrelid" + Environment.NewLine + 
            "JOIN pg_depend d ON d.refobjid = c.oid AND d.refobjsubid = a.attnum" + Environment.NewLine + 
            "JOIN pg_class seq ON seq.oid = d.objid AND seq.relkind = 'S'" + Environment.NewLine + 
            "JOIN pg_sequence s ON s.seqrelid = seq.oid" + Environment.NewLine + 
            "WHERE a.attrelid = @TableId" + Environment.NewLine + 
            "  AND a.attidentity IN ('a', 'd')" + Environment.NewLine + 
            "  AND NOT a.attisdropped";
    }

    public static string GetColumnInfo()
    {
        return
            "SELECT att.attnum AS column_id," + Environment.NewLine + 
            "       att.attname AS column_name," + Environment.NewLine + 
            "       typ.typname AS type_name," + Environment.NewLine + 
            "       CASE WHEN typ.typname IN ('varchar', 'bpchar') THEN (att.atttypmod - 4) ELSE att.attlen END AS length," + Environment.NewLine + 
            "       CASE WHEN typ.typname = 'numeric' THEN ((att.atttypmod - 4) >> 16) ELSE NULL END AS precision," + Environment.NewLine + 
            "       CASE WHEN typ.typname = 'numeric' THEN (att.atttypmod - 4) & 65535 ELSE NULL END AS scale," + Environment.NewLine + 
            "       NOT att.attnotnull AS is_nullable," + Environment.NewLine + 
            "       CASE WHEN co.collname = 'default' THEN db.datcollate ELSE co.collname END AS collation_name," + Environment.NewLine + 
            "       pg_get_expr(ad.adbin, ad.adrelid) AS column_default" + Environment.NewLine + 
            "FROM pg_attribute att" + Environment.NewLine + 
            "JOIN pg_class cls ON cls.oid = att.attrelid" + Environment.NewLine + 
            "JOIN pg_type typ ON typ.oid = att.atttypid" + Environment.NewLine + 
            "LEFT JOIN pg_attrdef ad ON ad.adrelid = att.attrelid AND ad.adnum = att.attnum" + Environment.NewLine + 
            "LEFT JOIN pg_collation co ON att.attcollation = co.oid" + Environment.NewLine + 
            "JOIN pg_database db ON db.datname = current_database()" + Environment.NewLine + 
            "WHERE att.attnum > 0 AND NOT att.attisdropped and cls.oid = @TableId" + Environment.NewLine + 
            "ORDER BY att.attnum";
    }

    public static string GetIndexInfo()
    {
        return
            "SELECT" + Environment.NewLine + 
            "    i.relname                     AS index_name," + Environment.NewLine + 
            "    am.amname                     AS access_method," + Environment.NewLine + 
            "    ix.indisprimary               AS is_primary," + Environment.NewLine + 
            "    ix.indisunique                AS is_unique," + Environment.NewLine + 
            "    ix.indisvalid                 AS is_valid," + Environment.NewLine + 
            "    ix.indisclustered             AS is_clustered," + Environment.NewLine + 
            "    cols.ord::int                 AS column_position," + Environment.NewLine + 
            "    a.attname                     AS column_name,          -- NULL for expression" + Environment.NewLine + 
            "    (ix.indoption[cols.ord::int] & 1) = 1 AS desc_order,   -- DESC flag" + Environment.NewLine + 
            "    (ix.indoption[cols.ord::int] & 2) = 2 AS nulls_first,  -- NULLS FIRST flag" + Environment.NewLine + 
            "    pg_get_indexdef(ix.indexrelid, cols.ord::int, true) AS column_expression" + Environment.NewLine + 
            "FROM pg_index ix" + Environment.NewLine + 
            "         JOIN pg_class i         ON i.oid = ix.indexrelid" + Environment.NewLine + 
            "         JOIN pg_class t         ON t.oid = ix.indrelid" + Environment.NewLine + 
            "         JOIN pg_namespace n     ON n.oid = t.relnamespace" + Environment.NewLine + 
            "         JOIN pg_am am           ON am.oid = i.relam" + Environment.NewLine + 
            "         LEFT JOIN pg_constraint c ON c.conindid = ix.indexrelid" + Environment.NewLine + 
            "         LEFT JOIN LATERAL unnest(ix.indkey) WITH ORDINALITY AS cols(attnum, ord) ON true" + Environment.NewLine + 
            "         LEFT JOIN pg_attribute a ON a.attrelid = t.oid AND a.attnum = cols.attnum" + Environment.NewLine + 
            "WHERE n.nspname = @SchemaName          " + Environment.NewLine + 
            "  AND t.relname = @TableName " + Environment.NewLine + 
            "  AND c.contype IS NULL " + Environment.NewLine + 
            "  AND ix.indisvalid " + Environment.NewLine + 
            "ORDER BY index_name, column_position;";
    }

    public static string GetIndexInfoWithoutColumnInfo()
    {
        return "SELECT" + Environment.NewLine + 
               "    ix.indrelid                   AS table_oid," + Environment.NewLine + 
               "    i.relname                     AS index_name," + Environment.NewLine + 
               "    ix.indexrelid                 AS indexrelid," + Environment.NewLine + 
               "    am.amname                     AS access_method," + Environment.NewLine + 
               "    ix.indisunique                AS is_unique," + Environment.NewLine + 
               "    ix.indisclustered             AS is_clustered" + Environment.NewLine + 
               "FROM pg_index ix" + Environment.NewLine + 
               "         JOIN pg_class i         ON i.oid = ix.indexrelid" + Environment.NewLine + 
               "         JOIN pg_class t         ON t.oid = ix.indrelid" + Environment.NewLine + 
               "         JOIN pg_namespace n     ON n.oid = t.relnamespace" + Environment.NewLine + 
               "         JOIN pg_am am           ON am.oid = i.relam" + Environment.NewLine + 
               "WHERE n.nspname = @SchemaName" + Environment.NewLine + 
               "  AND t.relname = @TableName" + Environment.NewLine +
               "  AND NOT ix.indisprimary" + Environment.NewLine +
               "  AND ix.indisvalid" + Environment.NewLine + 
               "ORDER BY index_name;";
    }

    public static string GetColumnInfoForIndex()
    {
        return "SELECT" + Environment.NewLine + 
               "    a.attname AS column_name," + Environment.NewLine + 
               "    cols.ord::int AS column_position," + Environment.NewLine + 
               "    (ix.indoption[cols.ord::int] & 1) = 1 AS is_descending," + Environment.NewLine + 
               "    (ix.indoption[cols.ord::int] & 2) = 2 AS nulls_first," + Environment.NewLine + 
               "    pg_get_indexdef(ix.indexrelid, cols.ord::int, true) AS column_expression" + Environment.NewLine + 
               "FROM pg_index ix" + Environment.NewLine + 
               "         LEFT JOIN LATERAL unnest(ix.indkey) WITH ORDINALITY AS cols(attnum, ord) ON true" + Environment.NewLine + 
               "         LEFT JOIN pg_attribute a ON a.attrelid = ix.indrelid AND a.attnum = cols.attnum" + Environment.NewLine + 
               "WHERE ix.indexrelid = @IndexRelId" + Environment.NewLine + 
               "ORDER BY column_position";
    }
}