using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;

public sealed class G35VerifyTokensHttpResponse
    : AppHttpResponse<object>
{
    private static readonly object _lock = new object();
    private static G35VerifyTokensHttpResponse _successInstance;
    private static G35VerifyTokensHttpResponse _failedInstance;

    /// <summary>
    ///     Return a success instance of G35VerifyAccessAndRefreshTokenHttpResponse class.
    /// </summary>
    public static G35VerifyTokensHttpResponse SUCCESS()
    {
        lock (_lock)
        {
            if (Equals(_successInstance, null))
            {
                _successInstance = new()
                {
                    HttpCode = StatusCodes.Status200OK,
                };
            }
        }

        return _successInstance;
    }

    /// <summary>
    ///     Return a failed instance of G35VerifyAccessAndRefreshTokenHttpResponse class.
    /// </summary>
    public static G35VerifyTokensHttpResponse FAILED()
    {
        lock (_lock)
        {
            if (Equals(_failedInstance, null))
            {
                _failedInstance = new()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                };
            }
        }

        return _failedInstance;
    }
}
