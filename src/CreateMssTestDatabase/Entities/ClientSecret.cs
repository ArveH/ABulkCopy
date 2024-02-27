namespace CreateMssTestDatabase.Entities;

[Table("ClientSecrets")]
public class ClientSecret : Secret
{
    [Required]
    [ForeignKey("Client")]
    public required string ClientId { get; set; }
}