using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatG34.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

public sealed class G34VerifyResetPasswordTokenHandler
    : IFeatureHandler<G34VerifyResetPasswordTokenRequest, G34VerifyResetPasswordTokenResponse>
{
    private readonly IG34Repository _repository;
    private readonly Lazy<IG34PreResetPasswordTokenHandler> _tokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G34VerifyResetPasswordTokenHandler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IG34PreResetPasswordTokenHandler> tokenHandler,
        Lazy<ISnowflakeIdGenerator> idGenerator
    )
    {
        _repository = unitOfWork.Value.G34Repository;
        _tokenHandler = tokenHandler;
        _idGenerator = idGenerator;
    }

    public async Task<G34VerifyResetPasswordTokenResponse> ExecuteAsync(
        G34VerifyResetPasswordTokenRequest request,
        CancellationToken ct
    )
    {
        // Extract token info from token.
        var (preResetPasswordTokenId, userId) =
            await _tokenHandler.Value.GetTokenInfoFromTokenAsync(request.ResetPasswordToken, ct);

        // Token is not valid
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

        var resetPasswordTokenId = _idGenerator.Value.Get().ToString();

        // Delete pre-token and create reset password token using the same token id.
        var dbResult = await _repository.SavePasswordResetTokenMetadatAsync(
            preResetPasswordTokenId,
            userId,
            resetPasswordTokenId,
            ct
        );

        if (!dbResult)
        {
            return new G34VerifyResetPasswordTokenResponse
            {
                StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.DATABASE_ERROR
            };
        }

        return new G34VerifyResetPasswordTokenResponse
        {
            StatusCode = G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS,
            Body = new() { ResetPasswordTokenId = resetPasswordTokenId }
        };
    }
}
