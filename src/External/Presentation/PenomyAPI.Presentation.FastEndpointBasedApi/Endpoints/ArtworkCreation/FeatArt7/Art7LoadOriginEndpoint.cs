﻿using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7;

public sealed class Art7LoadOriginEndpoint : EndpointWithoutRequest<Art7LoadOriginHttpResponse>
{
    public override void Configure()
    {
        Get("art7/origins");

        AllowAnonymous();
    }

    public override async Task<Art7LoadOriginHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<Art4LoadOriginRequest, Art4LoadOriginResponse>(
                request: Art4LoadOriginRequest.Empty,
                ct: ct);

        var httpResponse = new Art7LoadOriginHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = featResponse.Origins.Select(origin => new OriginDto
            {
                Id = origin.Id.ToString(),
                Label = origin.CountryName,
                ImageUrl = origin.ImageUrl,
            })
        };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}