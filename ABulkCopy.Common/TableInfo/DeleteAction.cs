namespace ABulkCopy.Common.TableInfo;

public enum DeleteAction
{
    Cascade = 1,
    SetNull = 2,
    SetDefault = 3,
    NoAction = 99
}