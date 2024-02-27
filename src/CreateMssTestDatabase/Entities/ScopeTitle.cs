using CreateMssTestDatabase.Interface;

namespace CreateMssTestDatabase.Entities;

[DebuggerDisplay("{DisplayName} ({ScopeId})")]
public class ScopeTitle : IIdentifiable, IAuditItem
{
    public long Id { get; set; }

    [Required]
    [ForeignKey("Scope")]
    [StringLength(Constants.Data.NameLength)]
    public required string ScopeId { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Description { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? DisplayName { get; set; }

    [StringLength(Constants.Data.LanguageLength)]
    public string? Language { get; set; }

    public DateTime LastUpdate { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    public string GetId() => Id.ToString();
}