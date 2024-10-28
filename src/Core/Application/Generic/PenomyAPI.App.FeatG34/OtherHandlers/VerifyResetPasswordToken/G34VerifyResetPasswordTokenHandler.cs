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
        // Extract token id from token.
        var (preResetPasswordTokenId, userId) =
            await _tokenHandler.Value.GetTokenInfoFromTokenAsync(request.ResetPasswordToken, ct);

        if (string.IsNullOrWhiteSpace(preResetPasswordTokenId))
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // Is token found
        var isTokenFound = await _repository.IsTokenFoundByTokenIdAsync(
            preResetPasswordTokenId,
            ct
        );

        // Token is not found
        if (!isTokenFound)
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // TODO: Delete pre-token and create reset password token using the same token id.
        var passwordResetToken = await _repository.GenerateResetPasswordTokenAsync(
            preResetPasswordTokenId,
            userId,
            ct
        );

        if (string.IsNullOrWhiteSpace(passwordResetToken))
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.DATABASE_ERROR
            };
        }

        return new G34VerifyResetPasswordTokenResponse
        {
            StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS,
            ResetPasswordToken = passwordResetToken
        };
    }
}
