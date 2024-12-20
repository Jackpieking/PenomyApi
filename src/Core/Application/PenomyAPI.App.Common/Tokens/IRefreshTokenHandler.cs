namespace PenomyAPI.App.Common.Tokens;

/// <summary>
///     Represent refresh token generator interface.
/// </summary>
public interface IRefreshTokenHandler
{
    /// <summary>
    ///     Generate refresh token with given length.
    /// </summary>
    /// <param name="length">
    ///     Length of refresh token.
    /// </param>
    /// <returns>
    ///     A random string for refresh token
    ///     or empty string if validate fail.
    /// </returns>
    string Generate(int length);
}
