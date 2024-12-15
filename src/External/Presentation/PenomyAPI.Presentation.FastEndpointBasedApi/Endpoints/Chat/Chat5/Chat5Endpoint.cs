using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Chat5;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat5.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat5;

public class Chat5Endpoint : Endpoint<Chat5RequestDto, Chat5HttpResponse>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;


    public Chat5Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public override void Configure()
    {
        Post("/Chat5/chat-message/remove");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Chat5RequestDto>>();
        AllowFormData();
        AllowFileUploads();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for remove chat group message";
            summary.Description = "This endpoint is used for remove chat group message.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Chat5HttpResponse { AppCode = Chat5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Chat5HttpResponse> ExecuteAsync(
        Chat5RequestDto requestDto,
        CancellationToken ct
    )
    {
        List<AppFileInfo> mediaFiles = [];
        var stateBag = ProcessorState<StateBag>();


        var userPostId = _idGenerator.Value.Get();
        var userId = stateBag.AppRequest.UserId;
        Chat5Request request = new()
        {
            UserId = userId,
            MessageID = long.Parse(requestDto.MessageId)
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<Chat5Request, Chat5Response>(
            request,
            ct
        );

        var httpResponse = Chat5HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
