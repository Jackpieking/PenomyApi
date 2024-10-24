using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Caching;

internal sealed class G1CompleteRegistrationCachingPostProcessor
    : PostProcessor<
        G1CompleteRegistrationRequest,
        G1CompleteRegistrationStateBag,
        G1CompleteRegistrationHttpResponse
    >
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1CompleteRegistrationCachingPostProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PostProcessAsync(
        IPostProcessorContext<
            G1CompleteRegistrationRequest,
            G1CompleteRegistrationHttpResponse
        > context,
        G1CompleteRegistrationStateBag state,
        CancellationToken ct
    )
    {
        if (Equals(context.Response, default))
        {
            return;
        }

        // Set new cache if current app code is suitable.
        if (
            context.Response.AppCode.Equals(
                $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.USER_EXIST}"
            )
            || context.Response.AppCode.Equals(
                $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.INVALID_TOKEN}"
            )
            || context.Response.AppCode.Equals(
                $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.PASSWORD_INVALID}"
            )
        )
        {
            var responseCaching = new G1CompleteRegistrationHttpResponseCaching
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
