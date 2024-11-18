using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM7;
using PenomyAPI.App.SM7.OtherHandlers.CountGroups;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7;

public class SM7CountGroupsEndpoint : Endpoint<EmptyRequest, SM7CountGroupsHttpResponse>
{
    public override void Configure()
    {
        Get("/SM7/pagination");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<EmptyRequest>>();

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get joined groups";
            summary.Description = "This endpoint is used for user get joined groups";
            summary.Response<SM7HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM7ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM7CountGroupsHttpResponse> ExecuteAsync(
        EmptyRequest requestDto,
        CancellationToken ct
    )
    {
        StateBag stateBag = ProcessorState<StateBag>();

        var featRequest = new SM7CountGroupsRequest
        {
            UserId = stateBag.AppRequest.UserId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM7CountGroupsRequest, SM7CountGroupsResponse>(
            featRequest,
            ct
        );

        var httpResponse = new SM7CountGroupsHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new SM7PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        if (featResponse.TotalGroups > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalGroups = featResponse.TotalGroups;
            httpResponse.Body.TotalPages = (int)Math.Ceiling((decimal)featResponse.TotalGroups / SM7PaginationOptions.DEFAULT_PAGE_SIZE);
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
