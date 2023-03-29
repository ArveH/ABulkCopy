namespace ABulkCopy.TestData.Entities;

[DebuggerDisplay("{TenantId}")]
public class Tenant : IIdentifiable, IAuditItem
{
    [StringLength(Constants.Data.NameLength)]
    public required string TenantId { get; set; }

    public Boolean AllowPartialLogin { get; set; } = false;

    [StringLength(Constants.Data.NameLength)]
    public string? TenantName { get; set; }

    public bool Enabled { get; set; } = true;

    [StringLength(Constants.Data.NameLength)]
    public string? CompanyName { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Description { get; set; }

    public ICollection<Domain>? Domains { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    public DateTime LastUpdate { get; set; }

    public ICollection<Idp>? Idps { get; set; }

    public string GetId() => TenantId;
}