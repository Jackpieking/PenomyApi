using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4.Middlewares.Validation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Password;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4;

public class Admin4Endpoint : Endpoint<Admin4HttpRequest, Admin4HttpResponse>
{
    private readonly AppDbContext _context;
    private readonly ISnowflakeIdGenerator _idGenerator;
    private readonly IAppPasswordHasher _passwordHasher;
    private readonly AdminJwtAuthenticationOptions _jwtOption;

    public Admin4Endpoint(
        AppDbContext context,
        ISnowflakeIdGenerator idGenerator,
        IAppPasswordHasher passwordHasher,
        AdminJwtAuthenticationOptions jwtOption
    )
    {
        _context = context;
        _idGenerator = idGenerator;
        _passwordHasher = passwordHasher;
        _jwtOption = jwtOption;
    }

    public override void Configure()
    {
        Post("/Admin4");
        AllowAnonymous();
        DontThrowIfValidationFails();
        PreProcessor<Admin4ValidationPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for login as admin in admin page.";
            summary.Description = "This endpoint is used for login as admin in admin page.";
        });
    }

    public override async Task<Admin4HttpResponse> ExecuteAsync(
        Admin4HttpRequest req,
        CancellationToken ct
    )
    {
        // Is user found by email.
        var foundUser = await _context
            .Set<SystemAccount>()
            .Where(user => user.Email.Equals(req.Email))
            .Select(user => new SystemAccount
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            })
            .FirstOrDefaultAsync(ct);

        // User with email does not exist.
        if (Equals(foundUser, null))
        {
            await SendNotFoundAsync(ct);

            return null;
        }

        // Does password belong to user.
        var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(
            req.Password,
            foundUser.PasswordHash
        );

        if (!isPasswordCorrect)
        {
            // User password is uncorrect still can try to login again.
            await SendAsync(null, StatusCodes.Status422UnprocessableEntity, ct);

            return null;
        }

        // Generate access token.
        var newAccessToken = GenerateAccessToken(
            claims:
            [
                new(CommonValues.Claims.TokenIdClaim, _idGenerator.Get().ToString()),
                new(CommonValues.Claims.UserIdClaim, foundUser.Id.ToString()),
            ],
            additionalSecondsFromNow: 30 * 60 // 30 minutes
        );

        var httpResponse = new Admin4HttpResponse
        {
            Body = new Admin4HttpResponse.BodyDto
            {
                AccessToken = newAccessToken,
                UserInfo = new Admin4HttpResponse.BodyDto.UserInfoDto { Email = foundUser.Email }
            }
        };

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims, int additionalSecondsFromNow)
    {
        return JwtBearer.CreateToken(options: option =>
        {
            option.SigningKey = _jwtOption.IssuerSigningKey;
            option.ExpireAt = DateTime.UtcNow.AddSeconds(value: additionalSecondsFromNow);
            option.User.Claims.AddRange(collection: claims);
            option.Audience = _jwtOption.ValidAudience;
            option.Issuer = _jwtOption.ValidIssuer;
            option.SigningAlgorithm = SecurityAlgorithms.HmacSha256;
            option.CompressionAlgorithm = CompressionAlgorithms.Deflate;
            option.User.Claims.Add(
                item: new(
                    type: JwtRegisteredClaimNames.Iat,
                    value: DateTime.UtcNow.ToLongTimeString()
                )
            );
        });
    }
}
