namespace ABulkCopy.APostgres.Extensions;

public static class StringExtensions
{
    public static NpgsqlDbType GetNativeType(this string typeStr)
    {
        return typeStr switch
        {
            PgTypes.BigInt => NpgsqlDbType.Bigint,
            PgTypes.Int => NpgsqlDbType.Integer,
            PgTypes.Boolean => NpgsqlDbType.Boolean,
            PgTypes.ByteA => NpgsqlDbType.Bytea,
            PgTypes.Char => NpgsqlDbType.Char,
            PgTypes.Character => NpgsqlDbType.Char,
            PgTypes.CharacterVarying => NpgsqlDbType.Varchar,
            PgTypes.VarChar => NpgsqlDbType.Varchar,
            PgTypes.Date => NpgsqlDbType.Date,
            PgTypes.DoublePrecision => NpgsqlDbType.Double,
            PgTypes.Decimal => NpgsqlDbType.Numeric,
            PgTypes.Numeric => NpgsqlDbType.Numeric,
            PgTypes.Money => NpgsqlDbType.Money,
            PgTypes.Real => NpgsqlDbType.Real,
            PgTypes.SmallInt => NpgsqlDbType.Smallint,
            PgTypes.Text => NpgsqlDbType.Text,
            PgTypes.Time => NpgsqlDbType.Time,
            PgTypes.Timestamp => NpgsqlDbType.Timestamp,
            PgTypes.TimestampTz => NpgsqlDbType.TimestampTz,
            PgTypes.Uuid => NpgsqlDbType.Uuid,
            _ => throw new ArgumentOutOfRangeException(nameof(typeStr), typeStr)
        };
    }
}