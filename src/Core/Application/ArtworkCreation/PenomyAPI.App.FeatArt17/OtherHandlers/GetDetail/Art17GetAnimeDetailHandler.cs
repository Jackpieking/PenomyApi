using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;

public class Art17GetAnimeDetailHandler
    : IFeatureHandler<Art17GetAnimeDetailRequest, Art17GetAnimeDetailResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt17Repository _art17Repository;

    public Art17GetAnimeDetailHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art17GetAnimeDetailResponse> ExecuteAsync(
        Art17GetAnimeDetailRequest request,
        CancellationToken ct
    )
    {
        _art17Repository = _unitOfWork.Value.Art17Repository;

        var creatorHasPermission = await _art17Repository.CheckCreatorPermissionAsync(
            request.ArtworkId,
            request.GetCreatorId(),
            ct);

        if (!creatorHasPermission)
        {
            return Art17GetAnimeDetailResponse.CREATOR_HAS_NO_PERMISSION;
        }

        var animeDetail = await _art17Repository.GetAnimeDetailByIdAsync(request.ArtworkId, ct);

        return new Art17GetAnimeDetailResponse
        {
            AppCode = Art17GetDetailResponseAppCode.SUCCESS,
            AnimeDetail = animeDetail,
        };
    }
}
