using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG5;

public class G5Handler : IFeatureHandler<G5Request, G5Response>
{
    private readonly IG5Repository _IG5Repository;

    public G5Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _IG5Repository = unitOfWork.Value.FeatG5Repository;
    }

    public async Task<G5Response> ExecuteAsync(G5Request request, CancellationToken ct)
    {
        try
        {
            var invalidId = request.ComicId == default;

            if (invalidId)
            {
                return G5Response.INVALID_REQUEST;
            }

            // Check if the comic is existed or not.
            var isComicExisted = await _IG5Repository.IsArtworkExistAsync(request.ComicId, ct);

            if (!isComicExisted)
            {
                return G5Response.NOT_FOUND;
            }

            // Get the comic detail,
            Artwork comicDetail = await _IG5Repository.GetArtWorkDetailByIdAsync(
                request.ComicId,
                ct);

            var isInUserFavoriteList = await _IG5Repository.IsComicInUserFavoriteListAsync(
                request.UserId,
                request.ComicId,
                ct);

            return new G5Response
            {
                IsSuccess = true,
                IsUserFavorite = isInUserFavoriteList,
                Result = comicDetail,
                StatusCode = G5ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return G5Response.FAILED;
        }
    }
}
