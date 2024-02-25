namespace ABulkCopy.Common.Types;

public enum UpdateAction
{
    Cascade = 1,
    SetNull = 2,
    SetDefault = 3,
    NoAction = 99
}