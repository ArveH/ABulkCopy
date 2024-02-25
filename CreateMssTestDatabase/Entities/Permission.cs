namespace CreateMssTestDatabase.Entities;

[Table("Permissions")]
[DebuggerDisplay("{SubjectId}, {ClientId}")]
public class Permission
{
    [Key]
    [StringLength(Constants.Data.NameLength)]
    public required string SubjectId { get; set; }

    [Key]
    [StringLength(Constants.Data.NameLength)]
    public virtual required string ClientId { get; set; }

    [Required]
    [StringLength(Constants.Data.ValueLength)]
    public virtual required string Scopes { get; set; }
}