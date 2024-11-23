using System;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Common;

public sealed class AppGlobalExceptionHandler : IMiddleware
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AppGlobalExceptionHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        const string CommonErrorResponseMessage = "Something went wrong, please contact support.";

        try
        {
            await next(context);
        }
        catch (Exception)
        {
            context.Response.Clear();

            await context.Response.SendAsync(
                CommonErrorResponseMessage,
                StatusCodes.Status500InternalServerError
            );

            await context.Response.CompleteAsync();
        }
    }
}
