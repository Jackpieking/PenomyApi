using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Caching;

internal sealed class G1CompleteRegistrationCachingPreProcessor
    : PreProcessor<G1CompleteRegistrationRequest, G1CompleteRegistrationStateBag>
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1CompleteRegistrationCachingPreProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G1CompleteRegistrationRequest> context,
        G1CompleteRegistrationStateBag state,
        CancellationToken ct
    )
    {
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        state.CacheKey =
            $"{nameof(G1CompleteRegistrationHttpResponse)}_usertoken_{context.Request.PreRegistrationToken}";

        // Retrieve from cache.
        var cacheModel =
            await _cacheHandler.Value.GetAsync<G1CompleteRegistrationHttpResponseCaching>(
                state.CacheKey,
                ct
            );

        // Cache value does not exist.
        if (!Equals(cacheModel, AppCacheModel<G1CompleteRegistrationHttpResponseCaching>.NotFound))
        {
            await context.HttpContext.Response.SendAsync(
                cacheModel.Value,
                cacheModel.Value.HttpCode,
                cancellation: ct
            );

            context.HttpContext.MarkResponseStart();
        }
    }
}
