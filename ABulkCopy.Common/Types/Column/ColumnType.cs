namespace ABulkCopy.Common.Types.Column;

public enum ColumnType
{
    BigInt = 1,
    Int = 2,
    SmallInt = 3,
    TinyInt = 4,
    Bool = 10,
    Money = 20,
    SmallMoney = 21,
    Decimal = 22,
    Float = 23,
    SmallFloat = 24,
    Date = 30,
    DateTime = 31,
    DateTimeAlt = 32,
    SmallDateTime = 33,
    DateTimeOffset = 34,
    Time = 35,
    Char = 40,
    VarChar = 41,
    LongText = 42,
    NChar = 43,
    NVarChar = 44,
    NLongText = 45,
    Raw = 50,
    SmallRaw = 51,
    Guid = 60,
    TimeStamp = 62
}