namespace CreateMssTestDatabase.Type;

/// <summary>
///     OpenID Connect flows.
/// </summary>
public enum TOidcFlow
{
    /// <summary>
    /// Authorization code flow
    /// </summary>
    AuthorizationCode = 0,

    /// <summary>
    /// Implicit flow
    /// </summary>
    Implicit = 1,

    /// <summary>
    /// Hybrid flow
    /// </summary>
    Hybrid = 2,

    /// <summary>
    /// Client credentials flow
    /// </summary>
    ClientCredentials = 3,

    //U4IDS does not support Resource Owner flow
    //ResourceOwner = 4, 

    /// <summary>
    /// Custom grant flow
    /// </summary>
    Custom = 5,

    /// <summary>
    /// Authorization code flow with proof key
    /// </summary>
    AuthorizationCodeWithProofKey = 6,

    /// <summary>
    /// Hybrid flow with proof key
    /// </summary>
    HybridWithProofKey = 7
}