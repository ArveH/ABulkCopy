namespace ABulkCopy.TestData.Entities;

[Table("CacheCommands")]
public sealed class CacheCommand
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Command { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Protocol { get; set; }

    [EnumDataType(typeof(TCacheCommand.Type))]
    public TCacheCommand.Type Type { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Key { get; set; }

    public DateTime? CreatedAt { get; set; }
}