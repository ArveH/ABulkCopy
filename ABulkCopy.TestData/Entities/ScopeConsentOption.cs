namespace ABulkCopy.TestData.Entities;

[Table("ScopeConsentOptions")]
public class ScopeConsentOption
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Scope")]
    public required string ScopeId { get; set; }

    /// <summary>
    /// True to enable consent for the scope. Default to false.
    /// </summary>
    public bool RequireConsent { get; set; } = false;

    /// <summary>
    /// Consent description link. Will be shown in the consent view.
    /// </summary>
    [StringLength(Constants.Data.ValueLength)]
    public string? Link { get; set; }

    /// <summary>
    /// Description of the link property
    /// </summary>
    [StringLength(Constants.Data.ValueLength)]
    public string? LinkDescription { get; set; }

    public ScopeConsentOption Copy() => (ScopeConsentOption)MemberwiseClone();
}