namespace ABulkCopy.TestData.Entities;

[Table("Idps")]
[DebuggerDisplay("{Id}-{IdpName} ({TenantId})")]
public class Idp
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(Constants.Data.NameLength)]
    public required string IdpName { get; set; }

    [Required]
    [ForeignKey("Tenant")]
    public required string TenantId { get; set; }

    public bool IsDefault { get; set; } // One of the Idp's for a Tenant has to be default
    public bool Enabled { get; set; } = true;

    [StringLength(Constants.Data.UriLength)]
    public string? ImageUrl { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Description { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Authority { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? IdpRegId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? IdpSecret { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Protocol { get; set; }

    public ICollection<Unit4IdClaimType>? Unit4IdClaimType { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? NameClaimType { get; set; }

    public bool IncludeIdentityScopesInConsent { get; set; }

    public OpenIdConnectOptions? OpenIdConnectOptions { get; set; }
}