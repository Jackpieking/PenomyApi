using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4.OtherHandlers.TestingSnowflake;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4
{
    public sealed class Art4TestingEndpoint : EndpointWithoutRequest<IResult>
    {
        public override void Configure()
        {
            Get("/art4/test");

            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(CancellationToken ct)
        {
            var response = await FeatureExtensions.ExecuteAsync<Art4TestingRequest, Art4TestingResponse>(new(), ct);

            if (response.IsSuccess)
            {
                return TypedResults.Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    response.IsSuccess
                });
            }

            return TypedResults.BadRequest(new
            {
                StatusCode = StatusCodes.Status400BadRequest,
                response.IsSuccess
            });

        }
    }
}
