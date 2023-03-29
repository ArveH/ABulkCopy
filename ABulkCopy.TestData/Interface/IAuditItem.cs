namespace ABulkCopy.TestData.Interface;

public interface IAuditItem
{
    string? UserId { get; set; }
    string? UserName { get; set; }
    DateTime LastUpdate { get; set; }
}