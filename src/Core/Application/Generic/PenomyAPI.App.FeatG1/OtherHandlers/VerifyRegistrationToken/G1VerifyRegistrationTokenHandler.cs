using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

public sealed class G1VerifyRegistrationTokenHandler
    : IFeatureHandler<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenResponse>
{
    private readonly Lazy<IG1PreRegistrationTokenHandler> _tokenHandler;
    private readonly IG1Repository _repository;

    public G1VerifyRegistrationTokenHandler(
        Lazy<IG1PreRegistrationTokenHandler> tokenHandler,
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _tokenHandler = tokenHandler;
        _repository = unitOfWork.Value.G1Repository;
    }

    public async Task<G1VerifyRegistrationTokenResponse> ExecuteAsync(
        G1VerifyRegistrationTokenRequest request,
        CancellationToken ct
    )
    {
        // Extract email from token.
        var email = await _tokenHandler.Value.GetEmailFromTokenAsync(request.RegistrationToken, ct);

        if (string.IsNullOrWhiteSpace(email))
        {
            return new G1VerifyRegistrationTokenResponse
            {
                StatusCode = G1VerifyRegistrationTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // Is user email found
        var isUserFound = await _repository.IsUserFoundByEmailAsync(email, ct);

        // User already registered
        if (isUserFound)
        {
            return new G1VerifyRegistrationTokenResponse
            {
                StatusCode = G1VerifyRegistrationTokenResponseStatusCode.USER_EXIST,
            };
        }

        return new G1VerifyRegistrationTokenResponse
        {
            StatusCode = G1VerifyRegistrationTokenResponseStatusCode.SUCCESS
        };
    }
}
