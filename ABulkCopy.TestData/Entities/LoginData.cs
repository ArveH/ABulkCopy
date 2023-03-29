namespace ABulkCopy.TestData.Entities;

public class LoginData
{
    public long Id { get; set; }

    [Required]
    [ForeignKey("Login")]
    public long LoginId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public required string Key { get; set; }

    public string? Value { get; set; }
}