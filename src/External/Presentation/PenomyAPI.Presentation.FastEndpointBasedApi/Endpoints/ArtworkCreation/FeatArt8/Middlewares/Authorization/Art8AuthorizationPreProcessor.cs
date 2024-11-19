using FastEndpoints;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.Middlewares.Authorization;

public sealed class Art8AuthorizationPreProcessor
    : PreProcessor<Art8RequestDto, Art8StateBag>
{
    public override Task PreProcessAsync(
        IPreProcessorContext<Art8RequestDto> context,
        Art8StateBag state,
        CancellationToken ct)
    {
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Is token expired.
        if (tokenExpireTime < DateTime.UtcNow)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
