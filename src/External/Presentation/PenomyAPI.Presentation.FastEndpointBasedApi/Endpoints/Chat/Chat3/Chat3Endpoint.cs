using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Chat3;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3;

public class Chat3Endpoint : Endpoint<Chat3RequestDto, Chat3HttpResponse>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;


    public Chat3Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public override void Configure()
    {
        Post("/Chat3/chat/save");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Chat3RequestDto>>();
        AllowFormData();
        AllowFileUploads();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for save chat group message";
            summary.Description = "This endpoint is used for save chat group message.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Chat3HttpResponse { AppCode = Chat3ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Chat3HttpResponse> ExecuteAsync(
        Chat3RequestDto requestDto,
        CancellationToken ct
    )
    {
        List<AppFileInfo> mediaFiles = [];
        var stateBag = ProcessorState<StateBag>();


        var userPostId = _idGenerator.Value.Get();
        var userId = stateBag.AppRequest.UserId;
        Chat3Request request = new()
        {
            UserId = userId,
            MessageId = _idGenerator.Value.Get(),
            ChatGroupId = long.Parse(requestDto.ChatGroupId),
            Content = requestDto.Content,
            IsReply = requestDto.IsReply,
            MessageType = requestDto.MessageType
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<Chat3Request, Chat3Response>(
            request,
            ct
        );

        var httpResponse = Chat3HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        if (httpResponse.HttpCode == StatusCodes.Status200OK)
            httpResponse.Body = new Chat3ResponseDto
            {
                MessageId = featResponse.MessageId.ToString()
            };
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
