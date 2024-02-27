namespace CreateMssTestDatabase.Type;

public enum TPersistedGrant
{
    AuthorizationCode = 0,
    ReferenceToken = 1,
    RefreshToken = 2,
    UserConsent = 3,
    DeviceCode = 4,
    UserCode = 5
}