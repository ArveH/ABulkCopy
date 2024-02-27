namespace CreateMssTestDatabase.Type;

/// <summary>
/// 
/// </summary>
public enum TRefreshTokenUsage
{
    /// <summary>
    /// the refresh token handle will stay the same when refreshing tokens
    /// </summary>
    ReUse = 0,
    /// <summary>
    /// the refresh token handle will be updated when refreshing tokens
    /// </summary>
    OneTimeOnly = 1
}