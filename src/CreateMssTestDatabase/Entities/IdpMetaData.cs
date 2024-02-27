using CreateMssTestDatabase.Interface;

namespace CreateMssTestDatabase.Entities;

public class IdpMetaData : IIdentifiable, IAuditItem
{
    [Key]
    [StringLength(Constants.Data.UriLength)]
    public required string Id { get; set; }

    [Required]
    public required string Raw { get; set; }

    public DateTime? ValidTo { get; set; }

    public DateTime LastUsed { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }
    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }
    public DateTime LastUpdate { get; set; }

    public string GetId() => Id;
}