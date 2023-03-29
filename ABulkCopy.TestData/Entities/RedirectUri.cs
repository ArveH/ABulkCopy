namespace ABulkCopy.TestData.Entities;

[Table("RedirectUris")]
[DebuggerDisplay("{Uri}")]
public class RedirectUri : DomainUriBase
{
    public override int Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public override required string ClientId { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public override string? Uri { get; set; }
}