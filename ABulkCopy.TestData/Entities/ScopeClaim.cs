namespace ABulkCopy.TestData.Entities;

[Table("ScopeClaims")]
[DebuggerDisplay("{Name}")]
public class ScopeClaim
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Scope")]
    public required string ScopeId { get; set; }

    public bool AlwaysIncludeInIdToken { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Description { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Name { get; set; }
}