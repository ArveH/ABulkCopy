using System.Diagnostics.CodeAnalysis;

namespace ABulkCopy.TestData.Entities;

public class Login
{
    public long Id { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public required string U4SessionId { get; set; }

    [AllowNull]
    [StringLength(Constants.Data.NameLength)]
    public string SubjectId { get; set; }

    [AllowNull]
    [StringLength(Constants.Data.NameLength)]
    public string TenantId { get; set; }

    [AllowNull]
    [StringLength(Constants.Data.NameLength)]
    public string IdpName { get; set; }

    [AllowNull]
    [StringLength(Constants.Data.NameLength)]
    public string ClientId { get; set; }

    public DateTime LastUpdate { get; set; }

    public ICollection<LoginData>? LoginData { get; set; }
}