﻿namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresChar : PgTemplateStrColumn
{
    public PostgresChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, PgTypes.Char, name, isNullable, length, collation)
    {
    }

    public override string GetTypeClause()
    {
        if (Collation != null)
        {
            return $"{Type}({Length}) COLLATE {Collation}";
        }

        return $"{Type}({Length})";
    }
}