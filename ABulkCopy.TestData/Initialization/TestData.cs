namespace ABulkCopy.TestData.Initialization;

public static class TestData
{
    public static AllTypes AllTypes = new()
    {
        Id = 1,
        ExactNumBigInt = 123456789012345,
        ExactNumInt = 123456789,
        ExactNumSmallInt = 123456,
        ExactNumTinyInt = 123,
        ExactNumBit = true,
        ExactNumMoney = 1234567.12345m,
        ExactNumSmallMoney = 123.123m,
        ExactNumDecimal = 1234567.12345m,
        ExactNumNumeric = 1234567.12345m,
        ApproxNumFloat = 1234567.12345d,
        ApproxNumReal = 123.123f,
        DTDate = new DateTime(2023, 03, 31),
        DTDateTime = new DateTime(2023, 03, 31),
        DTDateTime2 = new DateTime(2023, 03, 31),
        DTSmallDateTime = new DateTime(2023, 03, 31),
        DTDateTimeOffset = new DateTimeOffset(new DateTime(2023, 03, 31), new TimeSpan(1, 0, 0)),
        DTTime = new TimeSpan(1, 2, 3, 4),
        CharStrChar20 = "12345678901234567890",
        CharStrVarchar20 = "12345678901234567890",
        CharStrVarchar10K = new string('a', 10000),
        CharStrNChar20 = "123456789ﯵ1234567890",
        CharStrNVarchar20 = "123456789ﯵ1234567890",
        CharStrNVarchar10K = new string('ﯵ', 10000),
        BinBinary5K = GetBytes(5000),
        BinVarbinary10K = GetBytes(10000),
        OtherGuid = new Guid("A17542D9-A61C-4E4C-8512-DAFFC1416142"),
        OtherXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> <SomeTag>A value</SomeTag>"
    };

    public static IEnumerable<Scope> StdScopes { get; } = new List<Scope>
    {
        CreateNewScope("openid",
            "Use your user identifier",
            "The application needs this to be able to securely identify you. If you do not grant this then you cannot use the application.",
            TScope.Identity,
            new List<ScopeClaim>()
            {
                new() {Id = 1, ScopeId = "openid", Name = "sub", AlwaysIncludeInIdToken = true},
                new() {Id = 2, ScopeId = "openid", Name = "tenant", AlwaysIncludeInIdToken = true},
                new() {Id = 3, ScopeId = "openid", Name = "unit4_id", AlwaysIncludeInIdToken = true}
            },
            CreateConsentOptions(1, "openId"),
            true),
        CreateNewScope("profile",
            "Use your user information",
            "Your user profile information (first name, last name, etc.).",
            TScope.Identity,
            new List<ScopeClaim>()
            {
                new() {Id = 4, ScopeId = "profile", Name = "name"},
                new() {Id = 5, ScopeId = "profile", Name = "family_name"},
                new() {Id = 6, ScopeId = "profile", Name = "given_name"},
                new() {Id = 7, ScopeId = "profile", Name = "middle_name"},
                new() {Id = 8, ScopeId = "profile", Name = "nickname"},
                new() {Id = 9, ScopeId = "profile", Name = "preferred_username"},
                new() {Id = 10, ScopeId = "profile", Name = "profile"},
                new() {Id = 11, ScopeId = "profile", Name = "picture"},
                new() {Id = 12, ScopeId = "profile", Name = "website"},
                new() {Id = 13, ScopeId = "profile", Name = "gender"},
                new() {Id = 14, ScopeId = "profile", Name = "birthdate"},
                new() {Id = 15, ScopeId = "profile", Name = "zoneinfo"},
                new() {Id = 16, ScopeId = "profile", Name = "locale"},
                new() {Id = 17, ScopeId = "profile", Name = "updated_at"}
            },
            CreateConsentOptions(2, "profile")
            ),
        CreateNewScope("email",
            "Use your email address",
            "",
            TScope.Identity,
            new List<ScopeClaim>()
            {
                new() {Id = 18, ScopeId = "email", Name = "email"},
                new() {Id = 19, ScopeId = "email", Name = "email_verified"}
            },
            CreateConsentOptions(3, "email")
            ),
        CreateNewScope("offline_access",
            "Remember you (offline access)",
            "Access to an application has limited lifetime. Letting the application remember you extends your access lifetime and for example allows you to login only once. The application may ask for permission on your behalf without prompting for permission (including when you are not present).",
            TScope.Resource,
            new List<ScopeClaim>(),
            CreateConsentOptions(4, "offline_access")),
         CreateNewScope("phone",
            "Use your phone number",
            "",
            TScope.Identity,
            new List<ScopeClaim>()
            {
                new() {Id = 20, ScopeId = "phone", Name = "phone_number"},
                new() {Id = 21, ScopeId = "phone", Name = "phone_number_verified"}
            },
            CreateConsentOptions(5, "phone")
         ),
        CreateNewScope("address",
            "Use your postal address",
            "",
            TScope.Identity,
            new List<ScopeClaim>()
            {
                new() {Id = 22, ScopeId = "address", Name = "address"}
            },
            CreateConsentOptions(6, "address")
        ),
        CreateNewScope("roles",
            "Access your externally assigned roles",
            "The application supports authorization wholly or in part based on the roles you have been assigned by your organization. Rights within the application might be limited if you fail to grant this.",
            TScope.Resource,
            new List<ScopeClaim>()
            {
                new() {Id = 23, ScopeId = "roles", Name = "role", Description = "External role claims", AlwaysIncludeInIdToken = false}
            },
            CreateConsentOptions(7, "roles")
        )
    };

    private static Scope CreateNewScope(
        string scopeId, string displayName, string description, TScope scopeType,
        List<ScopeClaim> scopeClaims,
        ScopeConsentOption consentOptions,
        bool required = false)
    {
        return new Scope
        {
            ScopeId = scopeId,
            ShowInDiscoveryDocument = true,
            AllowedForTenantSpecificClients = true,
            IsStandardScope = true,
            Enabled = true,
            Type = scopeType,
            DisplayName = displayName,
            Description = description,
            Required = required,
            Emphasize = true,
            ConsentOptions = consentOptions,
            Claims = scopeClaims
        };
    }

    private static ScopeConsentOption CreateConsentOptions(int id, string scopeId)
    {
        return new ScopeConsentOption { Id = id, ScopeId = scopeId, RequireConsent = true };
    }

    private static byte[] GetBytes(int size)
    {
        var bytes = new byte[size];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(i % 256);
        }
        return bytes;
    }
}