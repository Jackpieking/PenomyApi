using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4
{
    public sealed class Art4LoadOriginEndpoint : EndpointWithoutRequest<IResult>
    {
        public override void Configure()
        {
            Get("/art4/origins");

            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(CancellationToken ct)
        {
            var featResponse = await FeatureExtensions.ExecuteAsync<Art4LoadOriginRequest, Art4LoadOriginResponse>(
                    request: Art4LoadOriginRequest.Empty,
                    ct: ct);

            var responseBody = new Art4LoadOriginHttpResponse
            {
                HttpCode = StatusCodes.Status200OK,
                Body = featResponse.Origins.Select(origin => new OriginDto
                {
                    Id = origin.Id.ToString(),
                    Label = origin.CountryName,
                    ImageUrl = origin.ImageUrl,
                })
            };

            return Results.Ok(responseBody);
        }
    }
}
