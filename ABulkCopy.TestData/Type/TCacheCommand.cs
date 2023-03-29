namespace ABulkCopy.TestData.Type;

public static class TCacheCommand
{
    public struct Name
    {
        public const string Remove = "REMOVE";
    }

    public struct Protocol
    {
        public const string Saml2 = "SAML2";
    }

    public enum Type
    {
        IdentityProvider = 1,
        InMemory = 2,
        Redis = 3,
        ConfigManager = 4
    }
}