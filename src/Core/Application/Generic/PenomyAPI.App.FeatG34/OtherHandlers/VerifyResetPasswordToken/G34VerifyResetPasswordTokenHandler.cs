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
        // Extract token info from token.
        var (resetPasswordTokenId, userId) = await _tokenHandler.Value.GetTokenInfoFromTokenAsync(
            request.ResetPasswordToken,
            ct
        );

        // Token is not valid
        if (string.IsNullOrWhiteSpace(resetPasswordTokenId))
        {
            return new()
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // Is token found
        var isTokenFound = await _repository.IsTokenFoundByTokenIdAsync(resetPasswordTokenId, ct);

        // Token is not found
        if (!isTokenFound)
        {
            return new()
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            };
        }

        // Get user email by user id
        var userEmail = await _repository.GetUserEmailByUserIdAsync(long.Parse(userId), ct);

        return new()
        {
            StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS,
            Body = new() { Email = userEmail }
        };
    }
}
