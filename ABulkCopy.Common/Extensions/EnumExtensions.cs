namespace ABulkCopy.Common.Extensions;

public static class EnumExtensions
{
    public static string GetClause(this UpdateAction updateAction)
    {
        return updateAction switch
        {
            UpdateAction.Cascade => "ON UPDATE CASCADE",
            UpdateAction.SetNull => "ON UPDATE SET NULL",
            UpdateAction.SetDefault => "ON UPDATE SET DEFAULT",
            _ => ""
        };
    }

    public static string GetClause(this DeleteAction deleteAction)
    {
        return deleteAction switch
        {
            DeleteAction.Cascade => "ON DELETE CASCADE",
            DeleteAction.SetNull => "ON DELETE SET NULL",
            DeleteAction.SetDefault => "ON DELETE SET DEFAULT",
            _ => ""
        };
    }
}