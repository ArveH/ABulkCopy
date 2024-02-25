using System.Diagnostics.CodeAnalysis;

namespace CreateMssTestDatabase.Entities;

[ExcludeFromCodeCoverage]
[Table("Domains")]
[DebuggerDisplay("{DomainName}")]
public class Domain
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Tenant")]
    public required string TenantId { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? DomainName { get; set; }

}