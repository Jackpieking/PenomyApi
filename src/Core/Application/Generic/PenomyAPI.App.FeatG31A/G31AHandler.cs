using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG31A;

public sealed class G31AHandler : IFeatureHandler<G31ARequest, G31AResponse>
{
    private readonly Lazy<IAccessTokenHandler> _accessTokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;
    private readonly IG31ARepository _repository;

    public G31AHandler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IAccessTokenHandler> accessTokenHandler,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator
    )
    {
        _accessTokenHandler = accessTokenHandler;
        _snowflakeIdGenerator = snowflakeIdGenerator;
        _repository = unitOfWork.Value.G31ARepository;
    }

    public async Task<G31AResponse> ExecuteAsync(G31ARequest request, CancellationToken ct)
    {
        // Generating new Id for both refresh and access token.
        var oldTokenId = request.GetAccessTokenId();
        var newTokenId = _snowflakeIdGenerator.Value.Get().ToString();

        // Update both token id [refresh and access] of user.
        // Access and refresh token id is same.
        var dbResult = await _repository.UpdateRefreshTokenAsync(oldTokenId, newTokenId, ct);

        // Database error due to some reason.
        if (!dbResult)
        {
            return new() { StatusCode = G31AResponseStatusCode.DATABASE_ERROR };
        }

        // Init new access token.
        // Generate access token.
        var newAccessToken = _accessTokenHandler.Value.Generate(
            claims:
            [
                new(CommonValues.Claims.TokenIdClaim, newTokenId),
                new(CommonValues.Claims.UserIdClaim, request.GetUserId())
            ],
            additionalSecondsFromNow: 10 * 60 // 10 minutes
        );

        // Return new access token.
        return new()
        {
            StatusCode = G31AResponseStatusCode.SUCCESS,
            Body = new() { AccessToken = newAccessToken }
        };
    }
}
