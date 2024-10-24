using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG1.Infrastructures;

namespace PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

public sealed class G1VerifyRegistrationTokenHandler
    : IFeatureHandler<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenResponse>
{
    private readonly Lazy<IG1PreRegistrationTokenHandler> _tokenHandler;

    public G1VerifyRegistrationTokenHandler(Lazy<IG1PreRegistrationTokenHandler> tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    public async Task<G1VerifyRegistrationTokenResponse> ExecuteAsync(
        G1VerifyRegistrationTokenRequest request,
        CancellationToken ct
    )
    {
        // Extract email from token.
        var result = await _tokenHandler.Value.ValidateEmailConfirmationTokenAsync(
            request.RegistrationToken,
            ct
        );

        // Invalid token
        // Return false.
        if (!result)
        {
            return new G1VerifyRegistrationTokenResponse
            {
                StatusCode = G1VerifyRegistrationTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        return new G1VerifyRegistrationTokenResponse
        {
            StatusCode = G1VerifyRegistrationTokenResponseStatusCode.SUCCESS
        };
    }
}
