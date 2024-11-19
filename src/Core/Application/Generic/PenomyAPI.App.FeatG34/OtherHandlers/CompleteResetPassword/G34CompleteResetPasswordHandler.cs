using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.FeatG34.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public sealed class G34CompleteResetPasswordHandler
    : IFeatureHandler<G34CompleteResetPasswordRequest, G34CompleteResetPasswordResponse>
{
    private readonly IG34Repository _repository;
    private readonly Lazy<IG34PreResetPasswordTokenHandler> _tokenHandler;

    public G34CompleteResetPasswordHandler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IG34PreResetPasswordTokenHandler> tokenHandler
    )
    {
        _repository = unitOfWork.Value.G34Repository;
        _tokenHandler = tokenHandler;
    }

    public async Task<G34CompleteResetPasswordResponse> ExecuteAsync(
        G34CompleteResetPasswordRequest request,
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
            return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN, };
        }

        // Is token found
        var isTokenFound = await _repository.IsTokenFoundByTokenIdAsync(resetPasswordTokenId, ct);

        // Token is not found
        if (!isTokenFound)
        {
            return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN, };
        }

        var userIdAsNumber = long.Parse(userId);

        // Validate user password.
        var isPasswordValid = await _repository.ValidatePasswordAsync(
            new() { Id = userIdAsNumber },
            request.NewPassword
        );

        // Password is not valid
        if (!isPasswordValid)
        {
            return new()
            {
                StatusCode = G34CompleteResetPasswordResponseStatusCode.INVALID_PASSWORD
            };
        }

        // Update user password.
        var dbResult = await _repository.UpdatePasswordAsync(
            userIdAsNumber,
            request.NewPassword,
            resetPasswordTokenId,
            ct
        );

        // Database error due to some internal problem.
        if (!dbResult)
        {
            return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.DATABASE_ERROR };
        }

        return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.SUCCESS };
    }
}
