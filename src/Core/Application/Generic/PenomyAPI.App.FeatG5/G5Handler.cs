using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG5;

public class G5Handler : IFeatureHandler<G5Request, G5Response>
{
    private IG5Repository _IG5Repository;
    private IArtworkRepository _artworkRepository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G5Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G5Response> ExecuteAsync(G5Request request, CancellationToken ct)
    {
        try
        {
            var unitOfWork = _unitOfWork.Value;

            _artworkRepository = unitOfWork.ArtworkRepository;

            var invalidId = request.ComicId == default;

            if (invalidId)
            {
                return G5Response.INVALID_REQUEST;
            }

            // Check if the comic is existed or not.
            var isComicExisted = await _artworkRepository.IsArtworkAvailableToDisplayByIdAsync(
                request.ComicId,
                request.UserId,
                ct);

            if (!isComicExisted)
            {
                return G5Response.NOT_FOUND;
            }

            _IG5Repository = unitOfWork.FeatG5Repository;

            // Get the comic detail,
            G5ComicDetailReadModel comicDetail = await _IG5Repository.GetArtWorkDetailByIdAsync(
                request.ComicId,
                ct
            );

            return new G5Response
            {
                IsSuccess = true,
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
