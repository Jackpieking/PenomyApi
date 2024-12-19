using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;

public class Art20GetDetailToCreateChapHandler
    : IFeatureHandler<Art20GetDetailToCreateChapRequest, Art20GetDetailToCreateChapResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt20Repository _art20Repository;

    public Art20GetDetailToCreateChapHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art20GetDetailToCreateChapResponse> ExecuteAsync(
        Art20GetDetailToCreateChapRequest request,
        CancellationToken cancellationToken)
    {
        var unitOfWork = _unitOfWork.Value;

        _art20Repository = unitOfWork.Art20Repository;

        var isCreatorAuthorizedToAccess = await _art20Repository.IsCurrentCreatorAuthorizedToAccessAsync(
            request.GetCreatorId(),
            request.AnimeId,
            cancellationToken);

        if (!isCreatorAuthorizedToAccess)
        {
            return Art20GetDetailToCreateChapResponse.ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR;
        }

        // Get the comic detail to support create new chapter and return.
        var animeDetail = await _art20Repository.GetDetailToCreateChapterAsync(
            request.AnimeId,
            cancellationToken);

        return Art20GetDetailToCreateChapResponse.SUCCESS(animeDetail);
    }
}
