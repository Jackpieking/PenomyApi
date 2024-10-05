using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Handler : IFeatureHandler<G1Request, G1Response>
{
    private readonly Lazy<IG1Repository> _repository;
    private readonly Lazy<IG1PreRegistrationTokenHandler> _preRegistrationTokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;

    public G1Handler(
        Lazy<IG1Repository> repository,
        Lazy<IG1PreRegistrationTokenHandler> preRegistrationTokenHandler,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator
    )
    {
        _repository = repository;
        _preRegistrationTokenHandler = preRegistrationTokenHandler;
        _snowflakeIdGenerator = snowflakeIdGenerator;
    }

    public async Task<G1Response> ExecuteAsync(G1Request request, CancellationToken ct)
    {
        // Does user exist by email.
        var isUserFound = await _repository.Value.IsUserFoundByEmailQueryAsync(
            email: request.Email,
            ct: ct
        );
        // User with email already exists.
        if (isUserFound)
        {
            return new() { StatusCode = G1ResponseStatusCode.USER_EXIST };
        }

        // Generate pre-registration token.
        var preRegistrationToken = await _preRegistrationTokenHandler.Value.GetAsync(
            request.Email,
            ct
        );

        // Pre generate nick name for new user.
        // If by somehow id is duplicated (maybe checking db manually).
        // Please contact project manager.
        var preGenNickName = GenerateRandomNickNamebaseOnEmail(request.Email);

        return new()
        {
            PreRegistrationToken = preRegistrationToken,
            ConfirmedEmail = request.Email,
            PreGenNickName = preGenNickName,
            StatusCode = G1ResponseStatusCode.SUCCESS
        };
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
