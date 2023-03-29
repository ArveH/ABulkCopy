using System.Diagnostics.CodeAnalysis;

namespace ABulkCopy.TestData.Entities;

[ExcludeFromCodeCoverage]
[Table("Unit4IdClaimTypes")]
[DebuggerDisplay("{ClaimType}")]
public class Unit4IdClaimType
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Idp")]
    public int IdpId { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? ClaimType { get; set; }

}