using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG15;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG15;

public class G15Handler : IFeatureHandler<G15Request, G15Response>
{
    private readonly IG15Repository _IG15Repository;
    private readonly IUnitOfWork _unitOfWork;

    public G15Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
        _IG15Repository = _unitOfWork.G15Repository;
    }

    public async Task<G15Response> ExecuteAsync(G15Request request, CancellationToken ct)
    {
        try
        {
            var invalidId = request.Id == default;

            if (invalidId)
            {
                return G15Response.ID_NOT_FOUND;
            }

            // Check if the comic is existed or not.
            const long GUEST_ID = -1;

            var isComicExisted = await _unitOfWork.ArtworkRepository.IsArtworkAvailableToDisplayByIdAsync(
                request.Id,
                GUEST_ID,
                ct);

            if (!isComicExisted)
            {
                return G15Response.ID_NOT_FOUND;
            }

            // Get the comic detail,
            G15AnimeDetailReadModel animeDetail = await _IG15Repository.GetArtWorkDetailByIdAsync(
                request.Id,
                ct
            );

            return G15Response.SUCCESS(animeDetail);
        }
        catch
        {
            return G15Response.ID_NOT_FOUND;
        }
    }
}
