using System.Diagnostics.CodeAnalysis;

namespace CreateMssTestDatabase.Entities;

[ExcludeFromCodeCoverage]
[Table("CompanyIdClaimTypes")]
[DebuggerDisplay("{ClaimType}")]
public class CompanyIdClaimType
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Idp")]
    public int IdpId { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? ClaimType { get; set; }

}