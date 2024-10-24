using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.FeatG1;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Middlewares.Caching;

internal sealed class G1CachingPostProcessor
    : PostProcessor<G1HttpRequest, G1StateBag, G1HttpResponse>
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1CachingPostProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PostProcessAsync(
        IPostProcessorContext<G1HttpRequest, G1HttpResponse> context,
        G1StateBag state,
        CancellationToken ct
    )
    {
        if (Equals(context.Response, default))
        {
            return;
        }

        // Set new cache if current app code is suitable.
        if (context.Response.AppCode.Equals($"G1.{G1ResponseStatusCode.USER_EXIST}"))
        {
            var responseCaching = new G1HttpResponseCaching
            {
                AppCode = context.Response.AppCode,
                HttpCode = context.Response.HttpCode,
                Body = context.Response.Body as IEnumerable<string>,
                Errors = context.Response.Errors as IEnumerable<string>,
                ResponseTime = context.Response.ResponseTime
            };

            // Caching the return value.
            await _cacheHandler.Value.SetAsync(
                state.CacheKey,
                responseCaching,
                new() { DurationInSeconds = state.CacheDurationInSeconds },
                ct
            );
        }
    }
}
