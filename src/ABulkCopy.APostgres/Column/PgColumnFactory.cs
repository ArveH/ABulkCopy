namespace ABulkCopy.APostgres.Column;

public class PgColumnFactory : IPgColumnFactory
{
    public IColumn Create(
        int id,
        string name,
        string nativeType,
        int length,
        int? precision = null,
        int? scale = null,
        bool isNullable = false,
        string? collation = null)
    {
        return nativeType switch
        {
            PgTypes.BigInt => new PostgresBigInt(id, name, isNullable),
            PgTypes.Int => new PostgresInt(id, name, isNullable),
            PgTypes.Boolean => new PostgresBoolean(id, name, isNullable),
            PgTypes.ByteA => new PostgresByteA(id, name, isNullable),
            PgTypes.Char => new PostgresChar(id, name, isNullable, length, collation),
            PgTypes.Character => new PostgresChar(id, name, isNullable, length, collation),
            PgTypes.CharacterVarying => new PostgresVarChar(id, name, isNullable, length, collation),
            PgTypes.VarChar => new PostgresVarChar(id, name, isNullable, length, collation),
            PgTypes.Date => new PostgresDate(id, name, isNullable),
            PgTypes.DoublePrecision => new PostgresDoublePrecision(id, name, isNullable),
            PgTypes.Decimal => new PostgresDecimal(id, name, isNullable,
                precision ?? throw new ArgumentNullException(
                    nameof(precision), $"precision can't be null for {nativeType}"), scale),
            PgTypes.Numeric => new PostgresDecimal(id, name, isNullable,
                precision ?? throw new ArgumentNullException(
                    nameof(precision), $"precision can't be null for {nativeType}"), scale),
            PgTypes.Money => new PostgresMoney(id, name, isNullable),
            PgTypes.Real => new PostgresReal(id, name, isNullable),
            PgTypes.SmallInt => new PostgresSmallInt(id, name, isNullable),
            PgTypes.Text => new PostgresText(id, name, isNullable),
            PgTypes.Time => new PostgresTime(id, name, isNullable, precision),
            PgTypes.Timestamp => new PostgresTimestamp(id, name, isNullable, precision),
            PgTypes.TimestampTz => new PostgresTimestampTz(id, name, isNullable, precision),
            PgTypes.Uuid => new PostgresUuid(id, name, isNullable),
            _ => throw new ArgumentOutOfRangeException(nameof(nativeType), nativeType, null)
        };
    }
}