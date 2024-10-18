using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Handler : IFeatureHandler<G1Request, G1Response>
{
    private readonly IG1Repository _repository;
    private readonly Lazy<IAccessTokenHandler> _accessToken;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;

    public G1Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IAccessTokenHandler> accessToken,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator
    )
    {
        _repository = unitOfWork.Value.G1Repository;
        _accessToken = accessToken;
        _snowflakeIdGenerator = snowflakeIdGenerator;
    }

    public async Task<G1Response> ExecuteAsync(G1Request request, CancellationToken ct)
    {
        // Does user exist by email.
        var isUserFound = await _repository.IsUserFoundByEmailQueryAsync(
            email: request.Email,
            ct: ct
        );
        // User with email already exists.
        if (isUserFound)
        {
            return new() { StatusCode = G1ResponseStatusCode.USER_EXIST };
        }

        // TODO: JWT generator
        // Generate pre-registration token.
        var preRegistrationToken = _accessToken.Value.Generate(
            [new(ClaimTypes.Email, request.Email)],
            30
        );

        return new() { };

        // TODO: add sending mail.
        // TODO: complete response
    }

    // Why creating interpolated string like this?
    // Because this article explains that this is faster from .NET 6
    //
    // - SOURCE: https://btburnett.com/csharp/2021/12/17/string-interpolation-trickery-and-magic-with-csharp-10-and-net-6
    private string GenerateRandomNickNamebaseOnEmail(string email)
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral(email);
        handler.AppendLiteral("_");
        handler.AppendFormatted(_snowflakeIdGenerator.Value.Get());

        return handler.ToStringAndClear();
    }
}
