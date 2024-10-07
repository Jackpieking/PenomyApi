using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailHandler
    : IFeatureHandler<Art7LoadComicDetailRequest, Art7LoadComicDetailResponse>
{
    private readonly IArt7Repository _art7Repository;

    public Art7LoadComicDetailHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art7Repository = unitOfWork.Value.Art7Repository;
    }

    public async Task<Art7LoadComicDetailResponse> ExecuteAsync(
        Art7LoadComicDetailRequest request,
        CancellationToken ct)
    {
        var comicId = request.ComicId;

        // Check if the input comic id is existed or not before getting detail.
        var isComicExisted = await _art7Repository.IsComicExistedByIdAsync(
            comicId: request.ComicId,
            cancellationToken: ct);

        if (!isComicExisted)
        {
            return Art7LoadComicDetailResponse.ComicIdNotFound;
        }

        var comicDetail = await _art7Repository.GetComicDetailByIdAsync(comicId, ct);

        return new Art7LoadComicDetailResponse
        {
            StatusCode = Art7LoadComicDetailResponseStatusCode.SUCCESS,
            ComicDetail = comicDetail,
        };
    }
}
