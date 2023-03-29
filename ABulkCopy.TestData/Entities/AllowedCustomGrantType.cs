namespace ABulkCopy.TestData.Entities;

[Table("AllowedCustomGrantTypes")]
public class AllowedCustomGrantType
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public required string ClientId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? GrantType { get; set; }
}