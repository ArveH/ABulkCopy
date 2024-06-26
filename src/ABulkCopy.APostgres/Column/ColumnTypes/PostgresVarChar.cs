﻿namespace ABulkCopy.APostgres.Column.ColumnTypes;

public class PostgresVarChar : PgTemplateStrColumn
{
    public PostgresVarChar(int id, string name, bool isNullable, int length, string? collation = null)
        : base(id, PgTypes.VarChar, name, isNullable, length, collation)
    {
        if (length is < 1 or > 10_485_760)
        {
            Type = PgTypes.Text;
            Length = 1_073_741_824;
        }
    }

    public override string GetTypeClause()
    {
        var type = Type;
        if (Length is > 0 and < 10_485_760)
        {
            type += $"({Length})";
        }

        if (Collation != null)
        {
            type += $" COLLATE {Collation}";
        }

        return type;
    }
}