namespace ABulkCopy.Common.Types.Column;

public static class MssTypes
{
    public const string BigInt = "bigint";
    public const string Binary = "binary";
    public const string Bit = "bit";
    public const string Char = "char";
    public const string Date = "date";
    public const string DateTime = "datetime";
    public const string DateTime2 = "datetime2";
    public const string DateTimeOffset = "datetimeoffset";
    public const string Decimal = "decimal";
    public const string Float = "float";
    public const string Image = "image";
    public const string Int = "int";
    public const string Money = "money";
    public const string NChar = "nchar";
    public const string Numeric = "numeric";
    public const string NText = "ntext";
    public const string NVarChar = "nvarchar";
    public const string Real = "real";
    public const string SmallDateTime = "smalldatetime";
    public const string SmallInt = "smallint";
    public const string SmallMoney = "smallmoney";
    public const string Text = "text";
    public const string Time = "time";
    public const string Timestamp = "timestamp";
    public const string TinyInt = "tinyint";
    public const string UniqueIdentifier = "uniqueidentifier";
    public const string VarBinary = "varbinary";
    public const string VarChar = "varchar";
    public const string Xml = "xml";

    public static IReadOnlyList<string> AllTypes = new List<string>
    {
        BigInt,
        Binary,
        Bit,
        Char,
        Date,
        DateTime,
        DateTime2,
        DateTimeOffset,
        Decimal,
        Float,
        Image,
        Int,
        Money,
        NChar,
        Numeric,
        NText,
        NVarChar,
        Real,
        SmallDateTime,
        SmallInt,
        SmallMoney,
        Text,
        Time,
        Timestamp,
        TinyInt,
        UniqueIdentifier,
        VarBinary,
        VarChar,
        Xml
    };
}