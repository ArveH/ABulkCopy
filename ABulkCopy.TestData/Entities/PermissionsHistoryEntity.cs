namespace ABulkCopy.TestData.Entities;

[Table("PermissionsHistory")]
[DebuggerDisplay("{Id}")]
public class PermissionsHistoryEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? TenantId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? SubjectId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? ClientId { get; set; }

    [Required]
    [StringLength(Constants.Data.NameLength)]
    public required string Action { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(Constants.Data.ValueLength)]
    public required string Scopes { get; set; }
}