using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.PreProcessors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35;

public sealed class G35VerifyTokensEndpoint
    : Endpoint<G35VerifyTokensDto, G35VerifyTokensHttpResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IG35Repository _g35Repository;

    public G35VerifyTokensEndpoint(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("g35/verify-tokens");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G35VerifyTokensPreProcessor>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for verify the access and refresh token included in the request";
            summary.Description = "This endpoint is used for verifying the access and refresh token included in the request.";
            summary.ExampleRequest = new() { };
            summary.Response<G35VerifyTokensHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public async override Task<G35VerifyTokensHttpResponse> ExecuteAsync(
        G35VerifyTokensDto requestDto,
        CancellationToken ct)
    {
        var preprocessState = ProcessorState<G35StateBag>();

        // If the request is unauthorized then not processed.
        if (!preprocessState.IsAuthorized)
        {
            return await SendFailedHttpResponseAsync(ct);
        }

        // Check if the provided refresh-token and access token are valid or not.
        _g35Repository = _unitOfWork.Value.G35Repository;

        var isValidTokens = await _g35Repository.IsRefreshTokenValidAsync(
            preprocessState.RefreshTokenId,
            requestDto.RefreshToken,
            preprocessState.UserId,
            ct);

        if (!isValidTokens)
        {
            return await SendFailedHttpResponseAsync(ct);
        }

        var successHttpResponse = G35VerifyTokensHttpResponse.SUCCESS();

        await SendAsync(successHttpResponse, successHttpResponse.HttpCode, ct);

        return successHttpResponse;
    }

    private async Task<G35VerifyTokensHttpResponse> SendFailedHttpResponseAsync(CancellationToken ct)
    {
        var failedHttpResponse = G35VerifyTokensHttpResponse.FAILED();

        await SendAsync(
            failedHttpResponse,
            failedHttpResponse.HttpCode,
            ct);

        return failedHttpResponse;
    }
}
