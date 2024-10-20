using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG1.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

public sealed class G1VerifyRegistrationTokenHandler
    : IFeatureHandler<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenResponse>
{
    private readonly Lazy<IG1PreRegistrationTokenHandler> _tokenHandler;

    public G1VerifyRegistrationTokenHandler(Lazy<IG1PreRegistrationTokenHandler> tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    public async Task<G1VerifyRegistrationTokenResponse> ExecuteAsync(G1VerifyRegistrationTokenRequest request, CancellationToken ct)
    {
        // Extract email from token.
        var result = await _tokenHandler.Value.GetEmailFromTokenAsync(
            request.RegistrationToken,
            ct
        );

        if (!result.IsSuccess)
        {
            return new G1VerifyRegistrationTokenResponse
            {
                IsValid = false,
            };
        }

        return new G1VerifyRegistrationTokenResponse
        {
            IsValid = true,
            Email = result.Value
        };
    }
}
