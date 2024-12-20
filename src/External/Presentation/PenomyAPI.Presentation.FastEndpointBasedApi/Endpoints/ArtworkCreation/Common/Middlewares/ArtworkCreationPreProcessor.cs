using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;

internal sealed class ArtworkCreationPreProcessor<TRequest> : PreProcessor<TRequest, StateBag>
    where TRequest : notnull
{
    private IUnitOfWork _unitOfWork;

    public override async Task PreProcessAsync(
        IPreProcessorContext<TRequest> context,
        StateBag stateBag,
        CancellationToken ct
    )
    {
        // Bypass if response has started.
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        _unitOfWork = context.HttpContext.Resolve<IUnitOfWork>();

        // Check if the current user has already registered as creator or not.
        var userId = stateBag.AppRequest.UserId;

        var hasUserRegisteredAsCreator =
            await _unitOfWork.CreatorRepository.HasUserAlreadyBecomeCreatorAsync(userId, ct);

        if (!hasUserRegisteredAsCreator)
        {
            var forbiddenHttpResponse = new AppHttpResponse<string>
            {
                HttpCode = StatusCodes.Status403Forbidden,
                Body = "User has not registered as creator"
            };

            await context.HttpContext.Response.SendAsync(
                forbiddenHttpResponse,
                forbiddenHttpResponse.HttpCode,
                null,
                ct
            );

            return;
        }
    }
}
