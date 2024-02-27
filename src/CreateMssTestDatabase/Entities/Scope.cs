using CreateMssTestDatabase.Interface;
using CreateMssTestDatabase.Type;

namespace CreateMssTestDatabase.Entities;

[DebuggerDisplay("{ScopeId} ({Type})")]
public class Scope : IIdentifiable, IAuditItem
{
    [Key]
    [StringLength(Constants.Data.NameLength)]
    public required string ScopeId { get; set; }

    public bool AllowUnrestrictedIntrospection { get; set; }

    public ICollection<ScopeClaim>? Claims { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? ClaimsRule { get; set; }

    public ScopeConsentOption? ConsentOptions { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Description { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? DisplayName { get; set; }

    public bool Emphasize { get; set; }

    public bool Enabled { get; set; }

    public bool IncludeAllClaimsForUser { get; set; }

    public bool AllowedForTenantSpecificClients { get; set; }

    public DateTime LastUpdate { get; set; }

    public bool Required { get; set; }

    public ICollection<ScopeSecret>? ScopeSecrets { get; set; }

    public bool ShowInDiscoveryDocument { get; set; }

    public bool IsStandardScope { get; set; }

    public TScope Type { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    // Used to handle the Many-To-Many relationship between Clients and Scopes
    public ICollection<ClientScope>? ScopeClients { get; set; }

    public string GetId() => ScopeId;

    public Scope Copy() => (Scope)MemberwiseClone();
}