namespace ABulkCopy.TestData.Entities;

[Table("ExternalUserProfiles")]
[DebuggerDisplay("{SubjectId}")]
public class ExternalUserProfile
{
    [Key]
    [StringLength(Constants.Data.NameLength)]
    public required string SubjectId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Provider { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Unit4Id { get; set; }

    public ICollection<ExternalUserClaim>? Claims { get; set; }
}