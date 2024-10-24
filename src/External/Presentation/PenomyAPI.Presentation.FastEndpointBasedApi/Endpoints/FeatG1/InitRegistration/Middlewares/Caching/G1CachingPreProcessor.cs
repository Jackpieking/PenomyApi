using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Middlewares.Caching;

internal sealed class G1CachingPreProcessor : PreProcessor<G1HttpRequest, G1StateBag>
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1CachingPreProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G1HttpRequest> context,
        G1StateBag state,
        CancellationToken ct
    )
    {
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        state.CacheKey = $"{nameof(G1HttpResponse)}_user_email_{context.Request.Email}";

        // Retrieve from cache.
        var cacheModel = await _cacheHandler.Value.GetAsync<G1HttpResponseCaching>(
            state.CacheKey,
            ct
        );

        // Cache value does not exist.
        if (!Equals(cacheModel, AppCacheModel<G1HttpResponseCaching>.NotFound))
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
