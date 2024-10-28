using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG34.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

public sealed class G34VerifyResetPasswordTokenHandler
    : IFeatureHandler<G34VerifyResetPasswordTokenRequest, G34VerifyResetPasswordTokenResponse>
{
    private readonly IG34Repository _repository;
    private readonly Lazy<IG34PreResetPasswordTokenHandler> _tokenHandler;

    public G34VerifyResetPasswordTokenHandler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IG34PreResetPasswordTokenHandler> tokenHandler
    )
    {
        _repository = unitOfWork.Value.G34Repository;
        _tokenHandler = tokenHandler;
    }

    public async Task<G34VerifyResetPasswordTokenResponse> ExecuteAsync(
        G34VerifyResetPasswordTokenRequest request,
        CancellationToken ct
    )
    {
        // Extract email from token.
        var email = await _tokenHandler.Value.GetEmailFromTokenAsync(
            request.ResetPasswordToken,
            ct
        );

        if (string.IsNullOrWhiteSpace(email))
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // Is user email found
        var isUserFound = await _repository.IsUserFoundByEmailAsync(email, ct);

        // User with email does not registered
        if (!isUserFound)
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.USER_NOT_EXIST,
            };
        }

        return new G34VerifyResetPasswordTokenResponse
        {
            StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS
        };
    }
}
