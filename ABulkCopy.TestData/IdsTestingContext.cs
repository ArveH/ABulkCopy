namespace ABulkCopy.TestData;

public class IdsTestingContext : DbContext
{
    public IdsTestingContext(DbContextOptions<IdsTestingContext> options)
        : base(options)
    {
    }

    public DbSet<AllTypes>? AllTypes { get; set; }
    public DbSet<Client>? ConfiguredClients { get; set; }
    public DbSet<ClientSecret>? ClientSecrets { get; set; }
    public DbSet<AllowedCorsOrigin>? AllowedCorsOrigins { get; set; }
    public DbSet<Scope>? ConfiguredScopes { get; set; }
    public DbSet<ScopeSecret>? ScopeSecrets { get; set; }
    public DbSet<ScopeConsentOption>? ScopeConsentOptions { get; set; }
    public DbSet<ClientScope>? ClientScope { get; set; }
    public DbSet<Tenant>? ConfiguredTenants { get; set; }
    public DbSet<Idp>? Idps { get; set; }
    public DbSet<Audit>? Audits { get; set; }
    public DbSet<PurgeSemaphore>? PurgeSemaphore { get; set; }
    public DbSet<UsageHistory>? UsageHistory { get; set; }
    public DbSet<ScopeTitle>? ScopeTitles{ get; set; }
    public DbSet<PersistedGrant>? PersistedGrants { get; set; }
    public DbSet<ExternalPersistedGrant>? ExternalPersistedGrants { get; set; }
    public DbSet<ExternalUserProfile>? ExternalUserProfiles { get; set; }
    public DbSet<ExternalUserClaim>? ExternalUserClaims { get; set; }
    public DbSet<Permission>? Permissions { get; set; }
    public DbSet<PermissionsHistoryEntity>? PermissionsHistory { get; set; }
    public DbSet<Login>? Logins { get; set; }
    public DbSet<IdpMetaData>? IdpMetaData { get; set; }
    public DbSet<CacheCommand>? CacheCommand { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientScope>()
            .HasKey(cs => new {cs.ClientId, Name = cs.ScopeId});

        modelBuilder.Entity<Permission>()
            .HasKey(p => new { p.SubjectId, p.ClientId });

        var enumConverter = new EnumToStringConverter<TPersistedGrant>();
        modelBuilder.Entity<PersistedGrant>()
            .Property(p => p.Type)
            .HasConversion(enumConverter);
        modelBuilder.Entity<ExternalPersistedGrant>()
            .Property(p => p.Type)
            .HasConversion(enumConverter);

        base.OnModelCreating(modelBuilder);
    }
}