using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Middlewares.Caching;

internal sealed class G1VerifyRegistrationTokenCachingPostProcessor
    : PostProcessor<
        G1VerifyRegistrationTokenRequest,
        G1VerifyRegistrationTokenStateBag,
        G1VerifyRegistrationTokenHttpResponse
    >
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1VerifyRegistrationTokenCachingPostProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PostProcessAsync(
        IPostProcessorContext<
            G1VerifyRegistrationTokenRequest,
            G1VerifyRegistrationTokenHttpResponse
        > context,
        G1VerifyRegistrationTokenStateBag state,
        CancellationToken ct
    )
    {
        if (Equals(context.Response, default))
        {
            return;
        }

        // Set new cache if current app code is suitable.
        if (false)
        {
            var responseCaching = new G1VerifyRegistrationTokenHttpResponseCaching
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
