using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.Sys1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Sys1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Sys1;

public class Sys1Endpoint : Endpoint<Sys1Request, Sys1HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    static Sys1Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public Sys1Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public override void Configure()
    {
        Post("/Sys1/sys-account/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Sys1Request>>();
        AllowFormData();
        AllowFileUploads();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating a new system account.";
            summary.Description = "This endpoint is used for creating new system account.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Sys1HttpResponse
                {
                    AppCode = Sys1ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<Sys1HttpResponse> ExecuteAsync(
        Sys1Request request,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var groupPostId = _idGenerator.Value.Get();
        var userId = stateBag.AppRequest.UserId;

        var featResponse = await FeatureExtensions.ExecuteAsync<Sys1Request, Sys1Response>(
            request,
            ct
        );

        var httpResponse = Sys1HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        httpResponse.Body = featResponse;
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
