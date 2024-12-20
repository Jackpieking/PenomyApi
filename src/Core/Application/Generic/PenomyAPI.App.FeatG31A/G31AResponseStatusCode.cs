namespace PenomyAPI.App.FeatG31A;

public enum G31AResponseStatusCode
{
    SUCCESS = 1,

    /// <summary>
    ///     Indicate when the refresh token is
    ///     not valid to process or already expired.
    /// </summary>
    UN_AUTHORIZED,

    /// <summary>
    ///     Indicate when the input access
    ///     token is not valid to process.
    /// </summary>
    INPUT_VALIDATION_FAIL,

    /// <summary>
    ///     Indicate when the input token is
    ///     not existed or not valid to process.
    /// </summary>
    FORBIDDEN,

    /// <summary>
    ///     Indicate when the input access token is not expired.
    /// </summary>
    ACCESS_TOKEN_IS_NOT_EXPIRED,

    /// <summary>
    ///     Indicate when the database operation is error.
    /// </summary>
    DATABASE_ERROR
}
