using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.Middlewares;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4;

public class G4Endpoint : Endpoint<G4RequestDto, G4HttpResponse>
{
    private readonly Lazy<IFusionCache> _fusionCache;
    private readonly Lazy<IFusionCacheSerializer> _serializer;

    public G4Endpoint(Lazy<IFusionCache> fusionCache, Lazy<IFusionCacheSerializer> serializer)
    {
        _fusionCache = fusionCache;
        _serializer = serializer;
    }

    public override void Configure()
    {
        Get("g4/recommended/artworks");

        AllowAnonymous();
        PreProcessor<G4AuthPreProcessor>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting recommended artworks by category.";
            summary.Description = "This endpoint is used for getting recommended artworks by category.";
            summary.Response<G4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G4HttpResponse> ExecuteAsync(
        G4RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G4StateBag>();

        // var httpResponse = await _fusionCache.Value.GetOrSetAsync(
        //     $"G4_input_{stateBag.IsAuthenticated}_{requestDto.GuestId}_{stateBag.UserId}",
        //     async ct =>
        //     {
        //         var httpResponse = await GetHttpResponseAsync(stateBag, requestDto, ct);

        //         return await _serializer.Value.SerializeAsync(httpResponse, ct);
        //     },
        //     options: new()
        //     {
        //         Duration = TimeSpan.FromSeconds(120),
        //         IsFailSafeEnabled = true,
        //         FailSafeMaxDuration = TimeSpan.FromSeconds(240),
        //         FailSafeThrottleDuration = TimeSpan.FromSeconds(30),
        //         FactorySoftTimeout = TimeSpan.FromSeconds(100),
        //         FactoryHardTimeout = TimeSpan.FromSeconds(500),
        //         AllowTimedOutFactoryBackgroundCompletion = true
        //     },
        //     ct
        // );

        // return await _serializer.Value.DeserializeAsync<G4HttpResponse>(httpResponse, ct);

        var httpResponse = await GetHttpResponseAsync(stateBag, requestDto, ct);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    private static async Task<G4HttpResponse> GetHttpResponseAsync(
        G4StateBag stateBag,
        G4RequestDto requestDto,
        CancellationToken ct
    )
    {
        var G4Request = new G4Request
        {
            ForSignedInUser = stateBag.IsAuthenticated,
            GuestId = requestDto.GuestId,
            UserId = stateBag.UserId,
            ArtworkType  = requestDto.ArtworkType,
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<G4Request, G4Response>(
            G4Request,
            ct
        );

        return G4HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G4Request, featResponse);
    }
}
