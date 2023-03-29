using System.Diagnostics.CodeAnalysis;

namespace ABulkCopy.TestData.Entities;

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