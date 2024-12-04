using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Infra.Configuration.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace PenomyAPI.Identity.AppAuthenticator;

public sealed class AccessTokenHandler : IAccessTokenHandler
{
    private readonly JwtAuthenticationOptions _option;

    public AccessTokenHandler(JwtAuthenticationOptions option)
    {
        _option = option;
    }

    public string Generate(IEnumerable<Claim> claims, int additionalSecondsFromNow)
    {
        return JwtBearer.CreateToken(options: option =>
        {
            option.SigningKey = _option.IssuerSigningKey;
            option.ExpireAt = DateTime.UtcNow.AddSeconds(value: additionalSecondsFromNow);
            option.User.Claims.AddRange(collection: claims);
            option.Audience = _option.ValidAudience;
            option.Issuer = _option.ValidIssuer;
            option.SigningAlgorithm = SecurityAlgorithms.HmacSha256;
            option.CompressionAlgorithm = CompressionAlgorithms.Deflate;
            option.User.Claims.Add(
                item: new(
                    type: JwtRegisteredClaimNames.Iat,
                    value: DateTime.UtcNow.ToLongTimeString()
                )
            );
            option.User.Claims.Add(
                item: new(
                    type: ClaimTypes.NameIdentifier,
                    value: claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)
                        .FirstOrDefault().Value
                )
            );
        });
    }
}
