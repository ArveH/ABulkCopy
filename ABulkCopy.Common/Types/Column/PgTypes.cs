namespace ABulkCopy.Common.Types.Column;

public static class PgTypes
{
    public const string BigInt = "bigint";
    public const string Boolean = "boolean";
    public const string ByteA = "bytea";
    public const string Char = "char";
    public const string Character = "character";
    public const string CharacterVarying = "character varying";
    public const string Date = "date";
    public const string Decimal = "decimal";
    public const string DoublePrecision = "double precision";
    public const string Int = "int";
    public const string Money = "money";
    public const string Numeric = "numeric";
    public const string Real = "real";
    public const string SmallInt = "smallint";
    public const string Text = "text";
    public const string Time = "time";
    public const string Timestamp = "timestamp";
    public const string TimestampTz = "timestamp with time zone";
    public const string Uuid = "uuid";
    public const string VarChar = "varchar";

    public static IReadOnlyList<string> AllTypes = new List<string>
    {
        BigInt,
        Boolean,
        ByteA,
        Char,
        Character,
        CharacterVarying,
        Date,
        Decimal,
        DoublePrecision,
        Int,
        Money,
        Numeric,
        Real,
        SmallInt,
        Text,
        Time,
        Timestamp,
        TimestampTz,
        Uuid,
        VarChar
    };
}