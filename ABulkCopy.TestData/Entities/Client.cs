namespace ABulkCopy.TestData.Entities;

[DebuggerDisplay("{ClientId} ({Flow})")]
public class Client : IIdentifiable, IAuditItem
{
    [StringLength(Constants.Data.NameLength)]
    public required string ClientId { get; set; }

    [Required]
    [StringLength(Constants.Data.NameLength)]
    public required string ClientName { get; set; }

    public int AbsoluteRefreshTokenLifetime { get; set; }

    public int AccessTokenLifetime { get; set; }

    public TAccessToken AccessTokenType { get; set; }

    public bool AllowAccessToAllScopes { get; set; } 

    public ICollection<AllowedCustomGrantType>? AllowedCustomGrantTypes { get; set; }

    public ICollection<AllowedCorsOrigin>? AllowedCorsOrigins { get; set; }

    public bool AlwaysSendClientClaims { get; set; }

    public int AuthorizationCodeLifetime { get; set; }

    public ICollection<ClientClaim>? Claims { get; set; }

    public ICollection<ClientSecret>? ClientSecrets { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? ClientUri { get; set; }

    public bool Enabled { get; set; }

    public TOidcFlow Flow { get; set; }

    public int IdentityTokenLifetime { get; set; }

    public bool IncludeJwtId { get; set; }

    public DateTime LastUpdate { get; set; }

    [ForeignKey("OwnerTenant")]
    public Tenant? Tenant { get; set; }

    public ICollection<PostLogoutRedirectUri>? PostLogoutRedirectUris { get; set; }

    public bool PrefixClientClaims { get; set; }

    public ICollection<RedirectUri>? RedirectUris { get; set; }

    public TRefreshTokenExpiration RefreshTokenExpiration { get; set; }

    public TRefreshTokenUsage RefreshTokenUsage { get; set; }

    public bool RequireConsent { get; set; }

    public int SlidingRefreshTokenLifetime { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserId { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? UserName { get; set; }

    // Used to handle the Many-To-Many relationship between Clients and Scopes
    public ICollection<ClientScope>? ClientScopes { get; set; }

    public string GetId() => ClientId;
}