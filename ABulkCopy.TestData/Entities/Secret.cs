namespace ABulkCopy.TestData.Entities;

public abstract class Secret : IIdentifiable, IAuditItem
{
    public int Id { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Description { get; set; }

    public DateTimeOffset? Expiration { get; set; }

    [Required]
    [StringLength(Constants.Data.ValueLength)]
    public required string Value { get; set; }

    public DateTime LastUpdate { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    public string GetId() => Id.ToString();
}