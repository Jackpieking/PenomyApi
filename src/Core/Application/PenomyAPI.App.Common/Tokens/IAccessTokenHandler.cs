using System.Collections.Generic;
using System.Security.Claims;

namespace PenomyAPI.App.Common.Tokens;

/// <summary>
///     Represent jwt generator interface.
/// </summary>
public interface IAccessTokenHandler
{
    /// <summary>
    ///     Generate jwt base on list of claims.
    /// </summary>
    /// <param name="claims">
    ///     List of user claims.
    /// </param>
    /// <param name="additionalSecondsFromNow">
    ///     Additional seconds from now to expire.
    /// </param>
    /// <returns>
    ///     A string having format of jwt
    ///     or empty string if validate fail.
    /// </returns>
    string Generate(IEnumerable<Claim> claims, int additionalSecondsFromNow);
}
