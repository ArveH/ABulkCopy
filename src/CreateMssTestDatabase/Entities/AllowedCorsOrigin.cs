namespace CreateMssTestDatabase.Entities;

[Table("AllowedCorsOrigins")]
[DebuggerDisplay("{Uri}")]
public class AllowedCorsOrigin : DomainUriBase
{
    public override int Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public override required string ClientId { get; set; }

    [Required]
    [StringLength(Constants.Data.UriLength)]
    public override string? Uri { get; set; }
}