namespace ABulkCopy.TestData.Entities;

public class OpenIdConnectOptions
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Idp")]
    public int IdpId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? ResponseType { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Scope { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? EndSessionEndpoint { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? AcrValues { get; set; }
}