using CreateMssTestDatabase.Type;

namespace CreateMssTestDatabase.Entities;

public class Grant
{
    [Key]
    public required string Key { get; set; }

    [Required]
    public TPersistedGrant Type { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? SubjectId { get; set; }

    [Required]
    [StringLength(Constants.Data.NameLength)]
    public required string ClientId { get; set; }

    [Required]
    public DateTime CreationTime { get; set; }

    public DateTime? Expiration { get; set; }

    [Required]
    public required string Data { get; set; }
}