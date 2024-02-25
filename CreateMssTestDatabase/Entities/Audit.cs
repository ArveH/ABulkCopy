using CreateMssTestDatabase.Type;

namespace CreateMssTestDatabase.Entities;

[Table("Audits")]
[DebuggerDisplay("{EntityId} ({EntityType})")]
public class Audit
{
    public long AuditId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    public DateTime LastUpdate { get; set; }

    public string? ClientId { get; set; }

    public string? CompanyId { get; set; }

    public string? Tenant { get; set; }

    public required TCrud Action { get; set; }

    public required TEntity EntityType { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? EntityId { get; set; }

    public byte[]? EntityCompressed { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? AuditStorePath { get; set; }
}