using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public sealed class G34CompleteResetPasswordHandler
    : IFeatureHandler<G34CompleteResetPasswordRequest, G34CompleteResetPasswordResponse>
{
    private readonly IG34Repository _repository;

    public G34CompleteResetPasswordHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.G34Repository;
    }

    public async Task<G34CompleteResetPasswordResponse> ExecuteAsync(
        G34CompleteResetPasswordRequest request,
        CancellationToken ct
    )
    {
        // Get token info by token id.
        var userIdByTokenId = await _repository.GetResetPasswordTokenInfoByTokenIdAsync(
            request.ResetPasswordTokenId,
            ct
        );

        if (userIdByTokenId == default)
        {
            return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN };
        }

        // find user id by email
        var userIdByEmail = await _repository.GetUserIdByEmailAsync(request.Email, ct);

        // Token is not valid because it does not match user id
        if (userIdByTokenId != userIdByEmail)
        {
            return new() { StatusCode = G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN };
        }

        // Validate user password.
        var isPasswordValid = await _repository.ValidatePasswordAsync(
            new() { Id = userIdByTokenId },
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
            userIdByTokenId,
            request.NewPassword,
            request.ResetPasswordTokenId,
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
